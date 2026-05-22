namespace Infernum.FPS.Weapons.Fsm
{
    public sealed class WeaponFireState : IWeaponState
    {
        public static readonly WeaponFireState Instance = new WeaponFireState();

        private WeaponFireState()
        {
        }

        public void Enter(WeaponStateContext context)
        {
            context.IsAnimationFinished = false;
            context.Animation?.PlayFire(() => context.IsAnimationFinished = true);
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
