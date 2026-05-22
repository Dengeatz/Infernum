using System.Collections.Generic;
using Infernum.FPS.Core;
using Infernum.FPS.Player;
using Infernum.FPS.Weapons.Config;
using Infernum.FPS.Weapons.Fsm;
using UnityEngine;

namespace Infernum.FPS.Weapons
{
    /// <summary>
    /// Оружие: конфиг, FSM (idle / fire / reload), урон и управление UI-анимацией.
    /// </summary>
    public class Weapon
    {
        private readonly WeaponStateContext _context;
        private readonly WeaponStateMachine _stateMachine;

        public Weapon(WeaponConfig config, IPlayerWeaponAnimationService animation)
        {
            Config = config;
            _context = new WeaponStateContext(config, animation);
            _stateMachine = new WeaponStateMachine(_context, WeaponIdleState.Instance);
        }

        public WeaponConfig Config { get; }
        public bool CanAct => _stateMachine.IsIdle;

        public void Tick(float deltaTime)
        {
            _stateMachine.Tick(deltaTime);
        }

        public bool TryFire(IReadOnlyList<RaycastHit> hits, GameObject instigator, Vector3 attackOrigin)
        {
            if (!CanAct)
            {
                return false;
            }

            ApplyDamage(hits, instigator);
            _context.SetFireRequest(hits, instigator, attackOrigin);
            return true;
        }

        public bool TryReload()
        {
            if (!CanAct)
            {
                return false;
            }

            _context.SetReloadRequest();
            return true;
        }

        protected virtual void ApplyDamage(IReadOnlyList<RaycastHit> hits, GameObject instigator)
        {
            if (hits == null || hits.Count == 0)
            {
                return;
            }

            RaycastHit closest = hits[0];
            for (int i = 1; i < hits.Count; i++)
            {
                if (hits[i].distance < closest.distance)
                {
                    closest = hits[i];
                }
            }

            if (closest.distance > Config.DamageDistance)
            {
                return;
            }

            IDamageable damageable = closest.collider.GetComponentInParent<IDamageable>();
            if (damageable == null || !damageable.IsAlive)
            {
                return;
            }

            damageable.TakeDamage(Config.Damage, instigator);
        }
    }
}
