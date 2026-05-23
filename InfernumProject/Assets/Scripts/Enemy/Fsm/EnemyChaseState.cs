using UnityEngine;

namespace Infernum.FPS.Enemy.Fsm
{
    /// <summary>
    /// Идёт к цели — move-анимация.
    /// </summary>
    public sealed class EnemyChaseState : IEnemyState
    {
        public static readonly EnemyChaseState Instance = new EnemyChaseState();

        private EnemyChaseState()
        {
        }

        public void Enter(EnemyStateContext context)
        {
            context.Animation?.PlayMove();
        }

        public void Tick(EnemyStateContext context, float deltaTime)
        {
            if (context.TryApplyHitKnockback(deltaTime))
            {
                return;
            }

            Transform target = context.PlayerTransform;
            if (target == null || context.Controller == null)
            {
                return;
            }

            Vector3 dir = target.position - context.Self.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f)
            {
                return;
            }

            dir.Normalize();
            Vector3 motion = dir * context.ChaseSpeed;
            if (!context.Controller.isGrounded)
            {
                motion.y = -9.81f;
            }
            else
            {
                motion.y = -2f;
            }

            context.Controller.Move(motion * deltaTime);
        }

        public void Exit(EnemyStateContext context)
        {
        }
    }
}
