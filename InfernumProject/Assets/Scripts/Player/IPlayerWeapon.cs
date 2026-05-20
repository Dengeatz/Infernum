using UnityEngine;

namespace Infernum.FPS.Player
{
    public interface IPlayerWeapon
    {
        bool Enabled { get; set; }
        void Tick(float deltaTime);
        void SetWeaponPrefab(GameObject prefabOrNull);
    }
}
