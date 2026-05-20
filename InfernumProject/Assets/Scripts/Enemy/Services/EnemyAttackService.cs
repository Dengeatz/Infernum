using Infernum.FPS.Core;
using UnityEngine;

namespace Infernum.FPS.Enemy.Services
{
    /// <summary>
    /// Атака в заданном радиусе с перезарядкой (отдельная ответственность от патруля).
    /// </summary>
    public sealed class EnemyAttackService : MonoBehaviour
    {
        [SerializeField] private float attackRange = 2.2f;
        [SerializeField] private float damage = 12f;
        [SerializeField] private float cooldownSeconds = 1.1f;

        private float _cooldownLeft;

        public float AttackRange => attackRange;

        public bool IsInRange(Transform target)
        {
            if (target == null)
            {
                return false;
            }

            Vector3 delta = target.position - transform.position;
            delta.y = 0f;
            return delta.sqrMagnitude <= attackRange * attackRange;
        }

        public bool TryAttack(Transform target, GameObject instigator, float deltaTime)
        {
            _cooldownLeft -= deltaTime;
            if (target == null || !IsInRange(target) || _cooldownLeft > 0f)
            {
                return false;
            }

            IDamageable damageable = target.GetComponentInParent<IDamageable>();
            if (damageable == null || !damageable.IsAlive)
            {
                return false;
            }

            damageable.TakeDamage(damage, instigator);
            _cooldownLeft = cooldownSeconds;
            return true;
        }
    }
}
