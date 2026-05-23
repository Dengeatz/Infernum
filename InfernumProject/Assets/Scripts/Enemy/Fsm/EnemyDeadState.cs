namespace Infernum.FPS.Enemy.Fsm
{
    public sealed class EnemyDeadState : IEnemyState
    {
        public static readonly EnemyDeadState Instance = new EnemyDeadState();

        private EnemyDeadState()
        {
        }

        public void Enter(EnemyStateContext context)
        {
            context.Animation?.PlayDead();
        }

        public void Tick(EnemyStateContext context, float deltaTime)
        {
        }

        public void Exit(EnemyStateContext context)
        {
        }
    }
}
