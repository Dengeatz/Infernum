namespace Infernum.FPS.Enemy.Services
{
    public interface IEnemyHitReactionService
    {
        bool IsKnockbackActive { get; }
        bool ApplyKnockbackMovement(float deltaTime);
        void Tick(float deltaTime);
    }
}
