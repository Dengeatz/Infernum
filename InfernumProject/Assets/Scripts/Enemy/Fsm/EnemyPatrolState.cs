namespace Infernum.FPS.Enemy.Fsm
{
    public sealed class EnemyPatrolState : IEnemyState
    {
        public static readonly EnemyPatrolState Instance = new EnemyPatrolState();

        private EnemyPatrolState()
        {
        }

        public void Enter(EnemyStateContext context)
        {
        }

        public void Tick(EnemyStateContext context, float deltaTime)
        {
            context.Patrol?.Tick(deltaTime, context.SpawnPosition);
        }

        public void Exit(EnemyStateContext context)
        {
        }
    }
}
