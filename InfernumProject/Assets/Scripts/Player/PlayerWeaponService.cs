using Infernum.FPS.Core;
using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Оружие как отдельный сервис: визуал в руках, синхронизация с якорем, стрельба рейкастом.
    /// </summary>
    public sealed class PlayerWeaponService : MonoBehaviour, IPlayerWeapon
    {
        [SerializeField] private Transform handAnchor;
        [SerializeField] private Transform playerRoot;
        [SerializeField] private Camera aimCamera;
        [SerializeField] private Vector3 localPositionOffset = new Vector3(0.35f, -0.25f, 0.55f);
        [SerializeField] private Vector3 localEulerOffset;
        [SerializeField] private float damage = 25f;
        [SerializeField] private float maxDistance = 80f;
        [SerializeField] private LayerMask hitMask = ~0;

        private GameObject _weaponVisual;
        private bool _enabled = true;

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        private void Awake()
        {
            if (playerRoot == null)
            {
                playerRoot = transform;
            }

            if (handAnchor == null)
            {
                handAnchor = playerRoot;
            }

            if (aimCamera == null)
            {
                aimCamera = FindAnyObjectByType<Camera>();
            }
        }

        public void SetWeaponPrefab(GameObject prefabOrNull)
        {
            if (_weaponVisual != null)
            {
                Destroy(_weaponVisual);
                _weaponVisual = null;
            }

            if (prefabOrNull == null)
            {
                return;
            }

            _weaponVisual = Instantiate(prefabOrNull, handAnchor);
            _weaponVisual.transform.localPosition = Vector3.zero;
            _weaponVisual.transform.localRotation = Quaternion.identity;
        }

        public void Tick(float deltaTime)
        {
            if (handAnchor == null)
            {
                return;
            }

            handAnchor.position = playerRoot.TransformPoint(localPositionOffset);
            handAnchor.rotation = playerRoot.rotation * Quaternion.Euler(localEulerOffset);

            if (_weaponVisual != null)
            {
                _weaponVisual.transform.SetPositionAndRotation(handAnchor.position, handAnchor.rotation);
            }

            if (!_enabled || aimCamera == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }

        private void Fire()
        {
            Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
                if (damageable != null && damageable.IsAlive)
                {
                    damageable.TakeDamage(damage, gameObject);
                }
            }
        }
    }
}
