using Infernum.FPS.Core;
using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Фасад игрока: собирает сервисы и отдаёт наружу стабильный контракт IPlayer (Facade + DIP).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private PlayerCamera playerCameraService;
        [SerializeField] private PlayerMovement playerMovementService;
        [SerializeField] private PlayerWeaponService playerWeaponService;
        [SerializeField] private Health health;

        private IPlayerCamera _camera;
        private IPlayerMovement _movement;
        private IPlayerWeapon _weapon;

        public Transform Transform => transform;

        public Camera ViewCamera => playerCameraService != null ? playerCameraService.Camera : null;

        public bool IsAlive => health == null || health.IsAlive;

        private void Awake()
        {
            _camera = playerCameraService != null ? playerCameraService : null;
            _movement = playerMovementService != null ? playerMovementService : null;
            _weapon = playerWeaponService != null ? playerWeaponService : null;

            if (health != null)
            {
                health.Died += OnPlayerDied;
            }
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died -= OnPlayerDied;
            }
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _movement?.Tick(dt);
            _camera?.Tick(dt);
            _weapon?.Tick(dt);
        }

        public void SetControlsEnabled(bool enabled)
        {
            if (_movement != null)
            {
                _movement.Enabled = enabled;
            }

            if (_camera != null)
            {
                _camera.Enabled = enabled;
            }

            if (_weapon != null)
            {
                _weapon.Enabled = enabled;
            }
        }

        public void SetWeaponVisual(GameObject weaponPrefabOrNull)
        {
            _weapon?.SetWeaponPrefab(weaponPrefabOrNull);
        }

        private void OnPlayerDied(GameObject _)
        {
            SetControlsEnabled(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
