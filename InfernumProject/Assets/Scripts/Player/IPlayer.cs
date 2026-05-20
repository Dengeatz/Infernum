using UnityEngine;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Фасад игрока для внешних систем (враги, UI, квесты, спавнеры).
    /// </summary>
    public interface IPlayer
    {
        Transform Transform { get; }
        Camera ViewCamera { get; }
        bool IsAlive { get; }

        void SetControlsEnabled(bool enabled);
        void SetWeaponVisual(GameObject weaponPrefabOrNull);
    }
}
