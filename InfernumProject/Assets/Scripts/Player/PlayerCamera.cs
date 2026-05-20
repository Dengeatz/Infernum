using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Управление обзором от первого лица (pitch на камере, yaw на корне ориентации).
    /// </summary>
    public sealed class PlayerCamera : MonoBehaviour, IPlayerCamera
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform yawPivot;
        [SerializeField] private Transform cameraAttach;
        [SerializeField] private float sensitivity = 1.5f;
        [SerializeField] private float minPitch = -85f;
        [SerializeField] private float maxPitch = 85f;

        private float _pitch;
        private float _yaw;
        private bool _enabled = true;

        public Camera Camera => playerCamera;

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = FindAnyObjectByType<Camera>();
            }

            if (yawPivot == null)
            {
                yawPivot = transform;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Tick(float deltaTime)
        {
            if (!_enabled || playerCamera == null)
            {
                return;
            }

            float mx = Input.GetAxisRaw("Mouse X") * sensitivity * 100f * deltaTime;
            float my = Input.GetAxisRaw("Mouse Y") * sensitivity * 100f * deltaTime;

            yawPivot.Rotate(0f, mx, 0f, Space.World);
            
            _pitch -= my;
            _yaw += mx;
            
            if (_yaw > 180f)
            {
                _yaw -= 360f;
            }
            
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
            playerCamera.transform.position = cameraAttach.position;
            playerCamera.transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
        
    }
}
