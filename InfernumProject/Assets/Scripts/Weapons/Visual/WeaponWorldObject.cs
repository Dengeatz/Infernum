using Infernum.FPS.Player;
using Infernum.FPS.Visual;
using Infernum.FPS.Weapons.Config;
using UnityEngine;

namespace Infernum.FPS.Weapons.Visual
{
    /// <summary>
    /// Оружие в мире: 2D-спрайт, billboard к игроку, подбор в руки (UI).
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class WeaponWorldObject : MonoBehaviour
    {
        [SerializeField] private WeaponConfig config;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private FacePlayerSprite facePlayerSprite;
        [SerializeField] private Collider pickupCollider;
        [SerializeField] private KeyCode pickupKey = KeyCode.E;
        [SerializeField] private float pickupRange = 2.5f;

        public WeaponConfig Config => config;
        public bool IsAvailable => gameObject.activeSelf;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (facePlayerSprite == null)
            {
                facePlayerSprite = GetComponent<FacePlayerSprite>();
            }

            ApplyDisplaySprite();
        }

        public void Initialize(WeaponConfig weaponConfig, Camera viewCamera = null)
        {
            config = weaponConfig;
            ApplyDisplaySprite();

            if (facePlayerSprite != null)
            {
                if (viewCamera != null)
                {
                    facePlayerSprite.SetCamera(viewCamera);
                }
            }
        }

        public static WeaponWorldObject Spawn(WeaponConfig weaponConfig, Vector3 position, Quaternion rotation, Camera viewCamera = null)
        {
            if (weaponConfig == null)
            {
                return null;
            }

            GameObject instance;
            if (weaponConfig.WorldPickupPrefab != null)
            {
                instance = Instantiate(weaponConfig.WorldPickupPrefab, position, rotation);
            }
            else
            {
                instance = CreateRuntimePickup(weaponConfig, position, rotation);
            }

            if (!instance.TryGetComponent(out WeaponWorldObject worldObject))
            {
                worldObject = instance.AddComponent<WeaponWorldObject>();
            }

            worldObject.Initialize(weaponConfig, viewCamera);
            return worldObject;
        }

        public void OnPickedUp()
        {
            gameObject.SetActive(false);
        }

        public void DropAt(Vector3 worldPosition)
        {
            transform.position = worldPosition;
            gameObject.SetActive(true);
            ApplyDisplaySprite();

            if (pickupCollider != null)
            {
                pickupCollider.enabled = true;
            }
        }

        private void Update()
        {
            if (!IsAvailable || config == null || !Input.GetKeyDown(pickupKey))
            {
                return;
            }

            PlayerWeaponService weaponService = FindFirstObjectByType<PlayerWeaponService>();
            if (weaponService == null || weaponService.HasWeaponEquipped)
            {
                return;
            }

            float sqrRange = pickupRange * pickupRange;
            float sqrDist = (weaponService.transform.position - transform.position).sqrMagnitude;
            if (sqrDist <= sqrRange)
            {
                weaponService.PickUpFromWorld(this);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!IsAvailable || config == null || !Input.GetKeyDown(pickupKey))
            {
                return;
            }

            if (!other.TryGetComponent(out PlayerWeaponService weaponService))
            {
                weaponService = other.GetComponentInParent<PlayerWeaponService>();
            }

            if (weaponService == null || weaponService.HasWeaponEquipped)
            {
                return;
            }

            weaponService.PickUpFromWorld(this);
        }

        private void ApplyDisplaySprite()
        {
            if (spriteRenderer == null || config == null)
            {
                return;
            }

            spriteRenderer.sprite = config.GetIdlePreviewSprite();
        }

        private static GameObject CreateRuntimePickup(WeaponConfig weaponConfig, Vector3 position, Quaternion rotation)
        {
            var go = new GameObject($"Weapon_{weaponConfig.name}");
            go.transform.SetPositionAndRotation(position, rotation);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = weaponConfig.GetIdlePreviewSprite();

            go.AddComponent<FacePlayerSprite>();

            var trigger = go.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 1.2f;

            return go;
        }
    }
}
