using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Арена-шутер в духе Doom (2016) / Eternal: быстрый разгон на земле,
    /// отзывчивое торможение и заметный air control со strafe-jump инерцией.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [Header("Скорость")]
        [SerializeField] private float moveSpeed = 11f;
        [SerializeField] private float jumpHeight = 1.35f;
        [SerializeField] private float gravity = -28f;

        [Header("Земля")]
        [SerializeField] private float groundAcceleration = 90f;
        [SerializeField] private float groundFriction = 65f;

        [Header("Воздух")]
        [SerializeField] [Range(0f, 1f)] private float airControl = 0.82f;
        [SerializeField] private float airAcceleration = 55f;
        [SerializeField] private float airFriction = 2f;
        [SerializeField] private float airSpeedCap = 13f;

        [Header("Рывок (Shift)")]
        [SerializeField] private float dashSpeed = 24f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 2.5f;

        [SerializeField] private Transform orientation;

        private CharacterController _controller;
        private Vector3 _horizontalVelocity;
        private Vector3 _dashDirection;
        private float _verticalVelocity;
        private float _dashTimeRemaining;
        private float _dashCooldownRemaining;
        private bool _enabled = true;

        public float DashCooldown => dashCooldown;
        public float DashCooldownRemaining => _dashCooldownRemaining;
        public float DashCooldownNormalized => dashCooldown > 0f ? 1f - _dashCooldownRemaining / dashCooldown : 1f;
        public bool IsDashReady => _dashCooldownRemaining <= 0f && _dashTimeRemaining <= 0f;

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            if (orientation == null)
            {
                orientation = transform;
            }
        }

        public void Tick(float deltaTime)
        {
            if (!_enabled || !_controller.enabled)
            {
                return;
            }

            _dashCooldownRemaining = Mathf.Max(0f, _dashCooldownRemaining - deltaTime);

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 wish = orientation.forward * v + orientation.right * h;
            bool hasInput = wish.sqrMagnitude > 0.0001f;
            Vector3 wishDir = hasInput ? wish.normalized : Vector3.zero;

            if (_dashTimeRemaining <= 0f
                && _dashCooldownRemaining <= 0f
                && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
            {
                TryStartDash(wish, hasInput);
            }

            if (_dashTimeRemaining > 0f)
            {
                TickDash(deltaTime);
                return;
            }

            if (_controller.isGrounded)
            {
                if (_verticalVelocity < 0f)
                {
                    _verticalVelocity = -2f;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                ApplyFriction(groundFriction, deltaTime);

                if (hasInput)
                {
                    Accelerate(wishDir, moveSpeed, groundAcceleration, deltaTime);
                }
            }
            else
            {
                ApplyFriction(airFriction, deltaTime);

                if (hasInput)
                {
                    float wishSpeed = moveSpeed * airControl;
                    Accelerate(wishDir, wishSpeed, airAcceleration, deltaTime);
                    ClampHorizontalSpeed(airSpeedCap);
                }
            }

            _verticalVelocity += gravity * deltaTime;

            Vector3 velocity = _horizontalVelocity;
            velocity.y = _verticalVelocity;
            _controller.Move(velocity * deltaTime);
        }

        /// <summary>
        /// Quake-style accelerate: наращивает скорость по желаемому направлению без мгновенного сброса инерции.
        /// </summary>
        private void Accelerate(Vector3 wishDir, float wishSpeed, float acceleration, float deltaTime)
        {
            float currentSpeed = Vector3.Dot(_horizontalVelocity, wishDir);
            float addSpeed = wishSpeed - currentSpeed;
            if (addSpeed <= 0f)
            {
                return;
            }

            float accelSpeed = acceleration * wishSpeed * deltaTime;
            if (accelSpeed > addSpeed)
            {
                accelSpeed = addSpeed;
            }

            _horizontalVelocity += wishDir * accelSpeed;
        }

        private void ApplyFriction(float friction, float deltaTime)
        {
            float speed = _horizontalVelocity.magnitude;
            if (speed < 0.05f)
            {
                _horizontalVelocity = Vector3.zero;
                return;
            }

            float drop = speed * friction * deltaTime;
            float newSpeed = Mathf.Max(speed - drop, 0f);
            _horizontalVelocity *= newSpeed / speed;
        }

        private void ClampHorizontalSpeed(float maxSpeed)
        {
            Vector3 flat = _horizontalVelocity;
            float sqr = flat.sqrMagnitude;
            float maxSqr = maxSpeed * maxSpeed;
            if (sqr > maxSqr && sqr > 0.0001f)
            {
                _horizontalVelocity = flat.normalized * maxSpeed;
            }
        }

        private void TryStartDash(Vector3 wish, bool hasInput)
        {
            _dashDirection = hasInput ? wish : orientation.forward;
            _dashDirection.y = 0f;

            if (_dashDirection.sqrMagnitude < 0.0001f)
            {
                _dashDirection = orientation.forward;
            }

            _dashDirection.Normalize();
            _horizontalVelocity = _dashDirection * dashSpeed;
            _dashTimeRemaining = dashDuration;
            _dashCooldownRemaining = dashCooldown;
        }

        private void TickDash(float deltaTime)
        {
            _dashTimeRemaining -= deltaTime;
            _horizontalVelocity = _dashDirection * dashSpeed;

            if (_controller.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -2f;
            }

            _verticalVelocity += gravity * deltaTime;

            Vector3 velocity = _horizontalVelocity;
            velocity.y = _verticalVelocity;
            _controller.Move(velocity * deltaTime);
        }
    }
}
