using UnityEngine;

namespace Infernum.FPS.Enemy.Animation
{
    [CreateAssetMenu(fileName = "EnemyAnimationConfig", menuName = "Infernum/Enemy/Enemy Animation Config")]
    public class EnemyAnimationConfig : ScriptableObject
    {
        [Header("Конфиги кадров")]
        [SerializeField] private EnemyIdleAnimationConfig idle;
        [SerializeField] private EnemyMoveAnimationConfig move;
        [SerializeField] private EnemyAttackAnimationConfig attack;
        [SerializeField] private EnemyDeadAnimationConfig dead;

        [Header("Интервалы кадров (сек)")]
        [SerializeField] private float idleFrameInterval = 0.35f;
        [SerializeField] private float moveFrameInterval = 0.2f;
        [SerializeField] private float attackFrameInterval = 0.25f;
        [SerializeField] private float deadFrameInterval = 0.4f;

        public EnemyIdleAnimationConfig Idle => idle;
        public EnemyMoveAnimationConfig Move => move;
        public EnemyAttackAnimationConfig Attack => attack;
        public EnemyDeadAnimationConfig Dead => dead;

        public float IdleFrameInterval => idleFrameInterval;
        public float MoveFrameInterval => moveFrameInterval;
        public float AttackFrameInterval => attackFrameInterval;
        public float DeadFrameInterval => deadFrameInterval;
    }
}
