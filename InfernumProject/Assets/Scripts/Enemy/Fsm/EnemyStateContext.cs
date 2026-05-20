using Infernum.FPS.Core;
using Infernum.FPS.Enemy.Services;
using Infernum.FPS.Enemy;
using Infernum.FPS.Player;
using UnityEngine;

namespace Infernum.FPS.Enemy.Fsm
{
    /// <summary>
    /// Общий контекст для состояний FSM (данные + сервисы, без логики переходов внутри сервисов).
    /// </summary>
    public sealed class EnemyStateContext
    {
        public EnemyStateContext(
            Transform self,
            CharacterController controller,
            EnemyPatrolService patrol,
            EnemyAttackService combat,
            Health health,
            float chaseRange,
            float chaseSpeed)
        {
            Self = self;
            Controller = controller;
            Patrol = patrol;
            Combat = combat;
            Health = health;
            ChaseRange = chaseRange;
            ChaseSpeed = chaseSpeed;
        }

        public Transform Self { get; }
        public CharacterController Controller { get; }
        public EnemyPatrolService Patrol { get; }
        public EnemyAttackService Combat { get; }
        public Health Health { get; }

        public float ChaseRange { get; }
        public float ChaseSpeed { get; }

        public IPlayer Player { get; private set; }
        public IPatrolZone PatrolZone { get; private set; }
        public Vector3 SpawnPosition { get; private set; }

        public Transform PlayerTransform => Player != null ? Player.Transform : null;

        public void Bind(IPlayer player, IPatrolZone zone, Vector3 spawnPosition)
        {
            Player = player;
            PatrolZone = zone;
            SpawnPosition = spawnPosition;
            Patrol?.BindZone(zone);
        }

        public bool HasAlivePlayer()
        {
            return Player != null && Player.IsAlive && Player.Transform != null;
        }

        public float SqrDistanceToPlayer()
        {
            if (!HasAlivePlayer())
            {
                return float.PositiveInfinity;
            }

            Vector3 d = PlayerTransform.position - Self.position;
            d.y = 0f;
            return d.sqrMagnitude;
        }

        public bool IsPlayerInChaseRange()
        {
            return SqrDistanceToPlayer() <= ChaseRange * ChaseRange;
        }
    }
}
