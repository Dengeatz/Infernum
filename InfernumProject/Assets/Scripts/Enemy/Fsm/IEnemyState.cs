namespace Infernum.FPS.Enemy.Fsm
{
    /// <summary>
    /// Состояние конечного автомата врага (OCP — новые состояния без правки машины).
    /// </summary>
    public interface IEnemyState
    {
        void Enter(EnemyStateContext context);
        void Tick(EnemyStateContext context, float deltaTime);
        void Exit(EnemyStateContext context);
    }
}
