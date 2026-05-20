using UnityEngine;

namespace Infernum.FPS.Enemy.Fsm
{
    /// <summary>
    /// FSM: единственная ответственность — выбор и исполнение текущего состояния.
    /// </summary>
    public sealed class EnemyStateMachine : MonoBehaviour
    {
        private EnemyStateContext _context;
        private IEnemyState _current;

        public IEnemyState Current => _current;

        public void Initialize(EnemyStateContext context, IEnemyState initialState)
        {
            _context = context;
            ForceState(initialState);
        }

        public void ForceState(IEnemyState newState)
        {
            if (_context == null)
            {
                return;
            }

            _current?.Exit(_context);
            _current = newState;
            _current?.Enter(_context);
        }

        private void Update()
        {
            if (_context == null || _current == null)
            {
                return;
            }

            if (_context.Health != null && !_context.Health.IsAlive)
            {
                enabled = false;
                return;
            }

            IEnemyState next = EvaluateTransition(_current, _context);
            if (!ReferenceEquals(next, _current))
            {
                ForceState(next);
            }

            _current.Tick(_context, Time.deltaTime);
        }

        private static IEnemyState EvaluateTransition(IEnemyState current, EnemyStateContext ctx)
        {
            if (!ctx.HasAlivePlayer())
            {
                return EnemyPatrolState.Instance;
            }

            bool inChase = ctx.IsPlayerInChaseRange();
            bool strictInAttack = ctx.Combat != null && ctx.Combat.IsInRange(ctx.PlayerTransform);

            if (current is EnemyPatrolState)
            {
                if (strictInAttack)
                {
                    return EnemyAttackState.Instance;
                }

                if (inChase)
                {
                    return EnemyChaseState.Instance;
                }

                return EnemyPatrolState.Instance;
            }

            if (current is EnemyChaseState)
            {
                if (strictInAttack)
                {
                    return EnemyAttackState.Instance;
                }

                if (!inChase)
                {
                    return EnemyPatrolState.Instance;
                }

                return EnemyChaseState.Instance;
            }

            if (current is EnemyAttackState)
            {
                if (strictInAttack)
                {
                    return EnemyAttackState.Instance;
                }

                if (inChase)
                {
                    return EnemyChaseState.Instance;
                }

                return EnemyPatrolState.Instance;
            }

            return EnemyPatrolState.Instance;
        }
    }
}
