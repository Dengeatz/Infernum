namespace Infernum.FPS.Weapons.Fsm
{
    public sealed class WeaponIdleState : IWeaponState
    {
        public static readonly WeaponIdleState Instance = new WeaponIdleState();

        private WeaponIdleState()
        {
        }

        public void Enter(WeaponStateContext context)
        {
            context.ClearRequests();
            context.Animation?.PlayIdle();
        }

        public void Tick(WeaponStateContext context, float deltaTime)
        {
        }

        public void Exit(WeaponStateContext context)
        {
        }
    }
}
