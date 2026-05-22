namespace Infernum.FPS.Weapons.Fsm
{
    public interface IWeaponState
    {
        void Enter(WeaponStateContext context);
        void Tick(WeaponStateContext context, float deltaTime);
        void Exit(WeaponStateContext context);
    }
}
