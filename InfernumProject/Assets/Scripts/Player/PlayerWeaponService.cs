using Infernum.FPS.Weapons;
using Infernum.FPS.Weapons.Config;
using Infernum.FPS.Weapons.Visual;
using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Оружейный сервис: экипировка, FSM оружия, Fire / Reload, UI-анимация через PlayerWeaponAnimationService.
    /// </summary>
    public sealed class PlayerWeaponService : MonoBehaviour, IPlayerWeapon
    {
        [SerializeField] private PlayerRaycastService raycastService;
        [SerializeField] private PlayerWeaponAnimationService weaponAnimationService;
        [SerializeField] private Camera viewCamera;
        [SerializeField] private KeyCode reloadKey = KeyCode.R;

        private Weapon _currentWeapon;
        private bool _enabled = true;

        public Weapon CurrentWeapon => _currentWeapon;
        public bool HasWeaponEquipped => _currentWeapon != null;

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        private void Awake()
        {
            if (raycastService == null)
            {
                raycastService = GetComponent<PlayerRaycastService>();
            }

            if (weaponAnimationService == null)
            {
                weaponAnimationService = GetComponent<PlayerWeaponAnimationService>();
            }

            if (weaponAnimationService == null)
            {
                weaponAnimationService = gameObject.AddComponent<PlayerWeaponAnimationService>();
            }

            if (viewCamera == null)
            {
                viewCamera = GetComponentInChildren<Camera>();
            }
        }

        public void EquipWeapon(WeaponConfig config)
        {
            SetEquippedWeapon(config, null);
        }

        public void PickUpFromWorld(WeaponWorldObject worldObject)
        {
            if (worldObject == null || !worldObject.IsAvailable)
            {
                return;
            }

            SetEquippedWeapon(worldObject.Config, worldObject);
        }

        public void DropCurrentWeapon(Vector3 worldDropPosition)
        {
            if (_currentWeapon == null)
            {
                return;
            }

            WeaponConfig config = _currentWeapon.Config;
            ClearEquippedState();
            WeaponWorldObject.Spawn(config, worldDropPosition, Quaternion.identity, viewCamera);
        }

        public void SetWeaponPrefab(GameObject prefabOrNull)
        {
            if (prefabOrNull == null)
            {
                ClearEquippedState();
                return;
            }

            if (!prefabOrNull.TryGetComponent(out WeaponWorldObject worldObject))
            {
                worldObject = prefabOrNull.GetComponentInChildren<WeaponWorldObject>();
            }

            if (worldObject != null)
            {
                PickUpFromWorld(worldObject);
            }
        }

        public void Tick(float deltaTime)
        {
            weaponAnimationService?.Tick(deltaTime);
            _currentWeapon?.Tick(deltaTime);

            if (!_enabled || _currentWeapon == null || raycastService == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                TryFire();
            }

            if (Input.GetKeyDown(reloadKey))
            {
                _currentWeapon.TryReload();
            }
        }

        private void SetEquippedWeapon(WeaponConfig config, WeaponWorldObject worldSource)
        {
            ClearEquippedState();

            if (config == null)
            {
                return;
            }

            weaponAnimationService.BindWeapon(config);
            _currentWeapon = new Weapon(config, weaponAnimationService);

            if (worldSource != null)
            {
                worldSource.OnPickedUp();
            }
        }

        private void ClearEquippedState()
        {
            _currentWeapon = null;
            weaponAnimationService?.Hide();
        }

        private void TryFire()
        {
            if (raycastService == null)
            {
                return;
            }
            
            _currentWeapon.TryFire(raycastService.Hits, gameObject, raycastService.LastRayOrigin);
        }
    }
}
