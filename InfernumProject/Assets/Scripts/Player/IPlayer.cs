using Infernum.FPS.Core;
using Infernum.FPS.Weapons.Config;
using Infernum.FPS.Weapons.Visual;
using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Фасад игрока для внешних систем (враги, UI, квесты, спавнеры).
    /// </summary>
    public interface IPlayer : IDamageable
    {
        Transform Transform { get; }
        Camera ViewCamera { get; }
        bool IsAlive { get; }

        void SetControlsEnabled(bool enabled);
        void EquipWeapon(WeaponConfig config);
        void PickUpWeapon(WeaponWorldObject worldObject);
        void DropWeapon(Vector3 worldDropPosition);
        void SetWeaponVisual(GameObject weaponPrefabOrNull);
    }
}
