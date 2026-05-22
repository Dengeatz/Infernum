using Infernum.FPS.Player;
using UnityEngine;

namespace Infernum.FPS.Visual
{
    /// <summary>
    /// Поворачивает 2D-спрайт на сцене лицом к игроку (к камере от первого лица или к корню игрока).
    /// </summary>
    public sealed class FacePlayerSprite : MonoBehaviour
    {
        [SerializeField] private Transform playerTarget;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private bool faceViewCamera = true;
        [SerializeField] private bool lockYAxis = true;

        private void Awake()
        {
            ResolveTargets();
        }

        private void LateUpdate()
        {
            if (playerTarget == null && playerCamera == null)
            {
                ResolveTargets();
            }

            if (faceViewCamera && playerCamera != null)
            {
                transform.rotation = playerCamera.transform.rotation;
                return;
            }

            if (playerTarget == null)
            {
                return;
            }

            Vector3 toPlayer = playerTarget.position - transform.position;
            if (lockYAxis)
            {
                toPlayer.y = 0f;
            }

            if (toPlayer.sqrMagnitude < 0.0001f)
            {
                return;
            }

            transform.rotation = Quaternion.LookRotation(-toPlayer.normalized, Vector3.up);
        }

        public void SetPlayer(Transform player)
        {
            playerTarget = player;
        }

        public void SetCamera(Camera camera)
        {
            playerCamera = camera;
            faceViewCamera = camera != null;
        }

        private void ResolveTargets()
        {
            if (playerTarget == null)
            {
                Player.Player player = FindFirstObjectByType<Player.Player>();
                if (player != null)
                {
                    playerTarget = player.transform;
                    if (playerCamera == null)
                    {
                        playerCamera = player.ViewCamera;
                    }
                }
            }

            if (playerCamera == null && playerTarget != null)
            {
                playerCamera = playerTarget.GetComponentInChildren<Camera>();
            }

            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }
    }
}
