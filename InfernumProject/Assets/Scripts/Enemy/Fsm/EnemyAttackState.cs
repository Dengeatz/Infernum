using UnityEngine;

namespace Infernum.FPS.Enemy.Fsm
{
    /// <summary>
    /// Атакует — attack-анимация.
    /// </summary>
    public sealed class EnemyAttackState : IEnemyState
    {
        public static readonly EnemyAttackState Instance = new EnemyAttackState();

        private EnemyAttackState()
        {
        }

        public void Enter(EnemyStateContext context)
        {
            context.Animation?.PlayAttack();
        }

        public void Tick(EnemyStateContext context, float deltaTime)
        {
            if (context.TryApplyHitKnockback(deltaTime))
            {
                return;
            }

            if (context.Combat == null)
            {
                return;
            }

            Transform player = context.PlayerTransform;
            if (player == null)
            {
                return;
            }

            Vector3 look = player.position - context.Self.position;
            look.y = 0f;
            if (look.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(look.normalized, Vector3.up);
                context.Self.rotation = Quaternion.Slerp(context.Self.rotation, targetRot, 10f * deltaTime);
            }

            context.Combat.TryAttack(player, context.Self.gameObject, deltaTime);
        }

        public void Exit(EnemyStateContext context)
        {
        }
    }
}
