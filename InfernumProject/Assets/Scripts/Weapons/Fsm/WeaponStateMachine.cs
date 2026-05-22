namespace Infernum.FPS.Weapons.Fsm
{
    public sealed class WeaponStateMachine
    {
        private readonly WeaponStateContext _context;
        private IWeaponState _current;

        public IWeaponState Current => _current;
        public bool IsIdle => _current is WeaponIdleState;

        public WeaponStateMachine(WeaponStateContext context, IWeaponState initialState)
        {
            _context = context;
            ForceState(initialState);
        }

        public void ForceState(IWeaponState newState)
        {
            _current?.Exit(_context);
            _current = newState;
            _current?.Enter(_context);
        }

        public void Tick(float deltaTime)
        {
            if (_current == null)
            {
                return;
            }

            IWeaponState next = EvaluateTransition(_current, _context);
            if (!ReferenceEquals(next, _current))
            {
                ForceState(next);
            }

            _current.Tick(_context, deltaTime);
        }

        private static IWeaponState EvaluateTransition(IWeaponState current, WeaponStateContext ctx)
        {
            if (current is WeaponIdleState)
            {
                if (ctx.FireRequested)
                {
                    return WeaponFireState.Instance;
                }

                if (ctx.ReloadRequested)
                {
                    return WeaponReloadState.Instance;
                }

                return WeaponIdleState.Instance;
            }

            if (current is WeaponFireState)
            {
                return ctx.IsAnimationFinished ? WeaponIdleState.Instance : WeaponFireState.Instance;
            }

            if (current is WeaponReloadState)
            {
                return ctx.IsAnimationFinished ? WeaponIdleState.Instance : WeaponReloadState.Instance;
            }

            return WeaponIdleState.Instance;
        }
    }
}
