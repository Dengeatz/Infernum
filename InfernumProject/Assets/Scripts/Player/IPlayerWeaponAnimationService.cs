using System;
using System.Collections.Generic;
using Infernum.FPS.Weapons.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Infernum.FPS.Player
{
    public interface IPlayerWeaponAnimationService
    {
        Image WeaponImage { get; }
        bool IsPlaying { get; }

        void BindWeapon(WeaponConfig config);
        void Hide();
        void PlayIdle();
        void PlayFire(Action onComplete);
        void PlayReload(Action onComplete);
        void Tick(float deltaTime);
    }
}
