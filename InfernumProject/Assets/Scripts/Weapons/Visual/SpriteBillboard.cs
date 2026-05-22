using Infernum.FPS.Visual;
using UnityEngine;

namespace Infernum.FPS.Weapons.Visual
{
    /// <summary>
    /// Обёртка над <see cref="FacePlayerSprite"/> для оружия в мире.
    /// </summary>
    [RequireComponent(typeof(FacePlayerSprite))]
    public sealed class SpriteBillboard : MonoBehaviour
    {
        private FacePlayerSprite _facePlayer;

        private void Awake()
        {
            _facePlayer = GetComponent<FacePlayerSprite>();
            if (_facePlayer == null)
            {
                _facePlayer = gameObject.AddComponent<FacePlayerSprite>();
            }
        }

        public void SetCamera(Camera camera)
        {
            if (_facePlayer == null)
            {
                _facePlayer = GetComponent<FacePlayerSprite>();
            }

            _facePlayer?.SetCamera(camera);
        }
    }
}
