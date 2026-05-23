namespace Infernum.FPS.Enemy.Fsm
{
    /// <summary>
    /// Патруль / стоит на месте — idle-анимация.
    /// </summary>
    public sealed class EnemyPatrolState : IEnemyState
    {
        public static readonly EnemyPatrolState Instance = new EnemyPatrolState();

        private EnemyPatrolState()
        {
        }

        public void Enter(EnemyStateContext context)
        {
            context.Animation?.PlayIdle();
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
