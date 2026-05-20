using Infernum.FPS.Core;
using Infernum.FPS.Enemy.Fsm;
using Infernum.FPS.Enemy.Patrol;
using Infernum.FPS.Enemy.Services;
using Infernum.FPS.Player;
using UnityEngine;

namespace Infernum.FPS.Enemy
{
    /// <summary>
    /// Фасад врага: связывает здоровье, сервисы и FSM, наружу отдаёт IEnemy.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class Enemy : MonoBehaviour, IEnemy
    {
        [Header("Сервисы")]
        [SerializeField] private Health health;
        [SerializeField] private EnemyPatrolService patrol;
        [SerializeField] private EnemyAttackService combat;
        [SerializeField] private EnemyStateMachine stateMachine;

        [Header("Параметры AI")]
        [SerializeField] private float chaseRange = 12f;
        [SerializeField] private float chaseSpeed = 4.5f;

        [Header("Опционально для сцены без спавнера")]
        [SerializeField] private Player.Player autoBindPlayer;
        [SerializeField] private WaypointPatrolZone defaultPatrolZone;

        private CharacterController _controller;
        private EnemyStateContext _context;
        private bool _configured;

        public Transform Transform => transform;

        public bool IsAlive => health == null || health.IsAlive;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            if (patrol == null)
            {
                patrol = GetComponent<EnemyPatrolService>();
            }

            if (combat == null)
            {
                combat = GetComponent<EnemyAttackService>();
            }

            if (stateMachine == null)
            {
                stateMachine = GetComponent<EnemyStateMachine>();
            }

            if (health == null)
            {
                health = GetComponent<Health>();
            }

            _context = new EnemyStateContext(
                transform,
                _controller,
                patrol,
                combat,
                health,
                chaseRange,
                chaseSpeed);

            if (health != null)
            {
                health.Died += OnDied;
            }
        }

        private void Start()
        {
            if (_configured)
            {
                return;
            }

            Player.Player player = autoBindPlayer != null ? autoBindPlayer : FindFirstObjectByType<Player.Player>();
            Configure(player, defaultPatrolZone);
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died -= OnDied;
            }
        }

        public void Configure(IPlayer player, IPatrolZone patrolZone)
        {
            _configured = true;
            _context.Bind(player, patrolZone, transform.position);
            stateMachine.Initialize(_context, EnemyPatrolState.Instance);
            stateMachine.enabled = true;
        }

        private void OnDied(GameObject _)
        {
            if (_controller != null)
            {
                _controller.enabled = false;
            }

            if (stateMachine != null)
            {
                stateMachine.enabled = false;
            }

            if (patrol != null)
            {
                patrol.enabled = false;
            }

            if (combat != null)
            {
                combat.enabled = false;
            }
        }
    }
}
