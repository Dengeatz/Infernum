using System.Collections.Generic;
using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Каждый кадр выпускает луч и хранит все попадания (только чтение снаружи).
    /// </summary>
    public sealed class PlayerRaycastService : MonoBehaviour, IPlayerRaycastService
    {
        [SerializeField] private Camera aimCamera;
        [SerializeField] private Transform rayOrigin;
        [SerializeField] private LayerMask hitMask = ~0;

        private readonly List<RaycastHit> _hits = new();
        private RaycastHit[] _buffer = new RaycastHit[32];

        public IReadOnlyList<RaycastHit> Hits => _hits;
        public Vector3 LastRayOrigin { get; private set; }
        public Vector3 LastRayDirection { get; private set; }

        private void Awake()
        {
            if (aimCamera == null)
            {
                aimCamera = FindAnyObjectByType<Camera>();
            }

            if (rayOrigin == null && aimCamera != null)
            {
                rayOrigin = aimCamera.transform;
            }
        }

        public void Tick()
        {
            _hits.Clear();

            if (rayOrigin == null)
            {
                return;
            }

            Vector3 origin = rayOrigin.transform.position;
            Vector3 direction = rayOrigin.transform.forward;


            // if (aimCamera != null)
            // {
            //     Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            //     origin = ray.origin;
            //     direction = ray.direction;
            // }
            // else if (rayOrigin != null)
            // {
            //     origin = rayOrigin.position;
            //     direction = rayOrigin.forward;
            // }
            // else
            // {
            //     return;
            // }

            LastRayOrigin = origin;
            LastRayDirection = direction;

            int hitCount = Physics.RaycastNonAlloc(
                origin,
                direction,
                _buffer,
                Mathf.Infinity,
                hitMask,
                QueryTriggerInteraction.Ignore);

            if (hitCount == _buffer.Length)
            {
                _buffer = Physics.RaycastAll(origin, direction, Mathf.Infinity, hitMask, QueryTriggerInteraction.Ignore);
                hitCount = _buffer.Length;
            }

            for (int i = 0; i < hitCount; i++)
            {
                _hits.Add(_buffer[i]);
            }

            _hits.Sort((a, b) => a.distance.CompareTo(b.distance));
        }
    }
}
