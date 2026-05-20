using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Отвечает только за перемещение по плоскости и гравитацию (SRP).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private float walkSpeed = 6f;
        [SerializeField] private float sprintSpeed = 9f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private Transform orientation;

        private CharacterController _controller;
        private float _verticalVelocity;
        private bool _enabled = true;

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

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            bool sprint = Input.GetKey(KeyCode.LeftShift);

            Vector3 move = orientation.forward * v + orientation.right * h;
            move = Vector3.ClampMagnitude(move, 1f);

            float speed = sprint ? sprintSpeed : walkSpeed;
            Vector3 velocity = move * speed;

            if (_controller.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = -2f;
            }

            _verticalVelocity += gravity * deltaTime;
            velocity.y = _verticalVelocity;

            _controller.Move(velocity * deltaTime);
        }
    }
}
