using Infernum.FPS.Weapons.Config;
using Infernum.FPS.Weapons.Visual;
using UnityEngine;

namespace Infernum.FPS.Player
{
    public interface IPlayerWeapon
    {
        bool Enabled { get; set; }
        bool HasWeaponEquipped { get; }

        void Tick(float deltaTime);
        void EquipWeapon(WeaponConfig config);
        void PickUpFromWorld(WeaponWorldObject worldObject);
        void DropCurrentWeapon(Vector3 worldDropPosition);
        void SetWeaponPrefab(GameObject prefabOrNull);
    }
}
