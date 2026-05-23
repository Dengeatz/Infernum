using Infernum.FPS.Core;
using UnityEngine;

namespace Infernum.FPS.Enemy.Services
{
    /// <summary>
    /// Реакция на урон: вспышка покраснения спрайта и короткий откат назад.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public sealed class EnemyHitReactionService : MonoBehaviour, IEnemyHitReactionService
    {
        [SerializeField] private Health health;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CharacterController characterController;

        [Header("Покраснение")]
        [SerializeField] private Color hitColor = new Color(1f, 0.25f, 0.25f, 1f);
        [SerializeField] private float flashDuration = 0.15f;

        [Header("Отталкивание")]
        [SerializeField] private float knockbackDistance = 0.75f;
        [SerializeField] private float knockbackDuration = 0.12f;

        private Color _baseColor = Color.white;
        private float _flashTimer;
        private float _knockbackTimer;
        private Vector3 _knockbackDirection;
        private float _knockbackSpeed;

        public bool IsKnockbackActive => _knockbackTimer > 0f;

        private void Awake()
        {
            if (health == null)
            {
                health = GetComponent<Health>();
            }

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }

            if (spriteRenderer != null)
            {
                _baseColor = spriteRenderer.color;
            }
        }

        private void OnEnable()
        {
            if (health != null)
            {
                health.DamageTaken += OnDamageTaken;
                health.Died += OnDied;
            }
        }

        private void OnDisable()
        {
            if (health != null)
            {
                health.DamageTaken -= OnDamageTaken;
                health.Died -= OnDied;
            }
        }

        public void Tick(float deltaTime)
        {
            UpdateFlash(deltaTime);
        }

        public bool ApplyKnockbackMovement(float deltaTime)
        {
            if (_knockbackTimer <= 0f || characterController == null)
            {
                return false;
            }

            _knockbackTimer -= deltaTime;
            float step = _knockbackSpeed * deltaTime;
            Vector3 motion = _knockbackDirection * step;

            if (!characterController.isGrounded)
            {
                motion.y = -9.81f * deltaTime;
            }
            else
            {
                motion.y = -2f * deltaTime;
            }

            characterController.Move(motion);

            if (_knockbackTimer <= 0f)
            {
                _knockbackTimer = 0f;
            }

            return true;
        }

        private void OnDamageTaken(float damage, GameObject instigator)
        {
            if (damage <= 0f || health == null || !health.IsAlive)
            {
                return;
            }

            TriggerFlash();
            StartKnockback(instigator);
        }

        private void OnDied(GameObject _)
        {
            _knockbackTimer = 0f;
            _flashTimer = 0f;
            ResetColor();
        }

        private void TriggerFlash()
        {
            _flashTimer = flashDuration;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = hitColor;
            }
        }

        private void StartKnockback(GameObject instigator)
        {
            _knockbackDirection = ResolveKnockbackDirection(instigator);
            _knockbackTimer = knockbackDuration;
            _knockbackSpeed = knockbackDuration > 0f ? knockbackDistance / knockbackDuration : knockbackDistance;
        }

        private Vector3 ResolveKnockbackDirection(GameObject instigator)
        {
            Vector3 direction;

            if (instigator != null)
            {
                direction = transform.position - instigator.transform.position;
            }
            else
            {
                direction = -transform.forward;
            }

            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = -transform.forward;
                direction.y = 0f;
            }

            return direction.normalized;
        }

        private void UpdateFlash(float deltaTime)
        {
            if (_flashTimer <= 0f)
            {
                return;
            }

            _flashTimer -= deltaTime;
            if (_flashTimer <= 0f)
            {
                ResetColor();
                return;
            }

            if (spriteRenderer == null)
            {
                return;
            }

            float t = _flashTimer / flashDuration;
            spriteRenderer.color = Color.Lerp(_baseColor, hitColor, t);
        }

        private void ResetColor()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = _baseColor;
            }
        }
    }
}
