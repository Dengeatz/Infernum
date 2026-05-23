using Infernum.FPS.Core;
using Infernum.FPS.Enemy.Config;
using Infernum.FPS.Enemy.Fsm;
using Infernum.FPS.Enemy.Patrol;
using Infernum.FPS.Enemy.Services;
using Infernum.FPS.Player;
using Infernum.FPS.Visual;
using UnityEngine;

namespace Infernum.FPS.Enemy
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Enemy : MonoBehaviour, IEnemy
    {
        [Header("Конфиг")]
        [SerializeField] private EnemyConfig enemyConfig;

        [Header("Сервисы")]
        [SerializeField] private Health health;
        [SerializeField] private EnemyPatrolService patrol;
        [SerializeField] private EnemyAttackService combat;
        [SerializeField] private EnemyAnimationService animationService;
        [SerializeField] private EnemyStateMachine stateMachine;
        [SerializeField] private FacePlayerSprite facePlayerSprite;

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
        public EnemyConfig Config => enemyConfig;
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

            if (animationService == null)
            {
                animationService = GetComponent<EnemyAnimationService>();
            }

            if (animationService == null)
            {
                animationService = gameObject.AddComponent<EnemyAnimationService>();
            }

            if (stateMachine == null)
            {
                stateMachine = GetComponent<EnemyStateMachine>();
            }

            if (health == null)
            {
                health = GetComponent<Health>();
            }

            if (facePlayerSprite == null)
            {
                facePlayerSprite = GetComponent<FacePlayerSprite>();
            }

            _context = new EnemyStateContext(
                transform,
                _controller,
                patrol,
                combat,
                health,
                animationService,
                enemyConfig,
                chaseRange,
                chaseSpeed);

            if (health != null)
            {
                health.Died += OnDied;
            }

            if (enemyConfig != null)
            {
                animationService.BindConfig(enemyConfig);
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

            if (facePlayerSprite != null && player != null)
            {
                facePlayerSprite.SetCamera(player.ViewCamera);
            }

            if (enemyConfig != null)
            {
                animationService.BindConfig(enemyConfig);
            }

            stateMachine.Initialize(_context, EnemyPatrolState.Instance);
            stateMachine.enabled = true;
        }

        public void SetConfig(EnemyConfig config)
        {
            enemyConfig = config;
            animationService?.BindConfig(config);
        }

        private void OnDied(GameObject _)
        {
            if (_controller != null)
            {
                _controller.enabled = false;
            }

            if (patrol != null)
            {
                patrol.enabled = false;
            }

            if (combat != null)
            {
                combat.enabled = false;
            }

            if (facePlayerSprite != null)
            {
                facePlayerSprite.enabled = false;
            }

            stateMachine?.ForceState(EnemyDeadState.Instance);
        }
    }
}
