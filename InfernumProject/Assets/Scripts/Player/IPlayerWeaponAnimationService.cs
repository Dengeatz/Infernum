using System;
using Infernum.FPS.Core.Animation;
using Infernum.FPS.Weapons.Config;
using UnityEngine.UI;

namespace Infernum.FPS.Player
{
    public interface IPlayerWeaponAnimationService : ISpriteAnimationService
    {
        Image WeaponImage { get; }

        void BindWeapon(WeaponConfig config);
        void Hide();
        void PlayIdle();
        void PlayFire(Action onComplete);
        void PlayReload(Action onComplete);
    }
}
