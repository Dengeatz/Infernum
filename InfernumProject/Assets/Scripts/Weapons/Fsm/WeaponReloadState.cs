namespace Infernum.FPS.Weapons.Fsm
{
    public sealed class WeaponReloadState : IWeaponState
    {
        public static readonly WeaponReloadState Instance = new WeaponReloadState();

        private WeaponReloadState()
        {
        }

        public void Enter(WeaponStateContext context)
        {
            context.IsAnimationFinished = false;
            context.Animation?.PlayReload(() => context.IsAnimationFinished = true);
        }

        public void Tick(WeaponStateContext context, float deltaTime)
        {
        }

        public void Exit(WeaponStateContext context)
        {
            context.ClearRequests();
        }
    }
}
