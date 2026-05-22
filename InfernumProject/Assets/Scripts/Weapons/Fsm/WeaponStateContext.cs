using System.Collections.Generic;
using Infernum.FPS.Player;
using Infernum.FPS.Weapons.Config;
using UnityEngine;

namespace Infernum.FPS.Weapons.Fsm
{
    public sealed class WeaponStateContext
    {
        public WeaponStateContext(WeaponConfig config, IPlayerWeaponAnimationService animation)
        {
            Config = config;
            Animation = animation;
        }

        public WeaponConfig Config { get; }
        public IPlayerWeaponAnimationService Animation { get; }

        public IReadOnlyList<RaycastHit> PendingHits { get; private set; }
        public GameObject Instigator { get; private set; }
        public Vector3 AttackOrigin { get; private set; }
        public bool FireRequested { get; private set; }
        public bool ReloadRequested { get; private set; }
        public bool IsAnimationFinished { get; set; }

        public void SetFireRequest(IReadOnlyList<RaycastHit> hits, GameObject instigator, Vector3 attackOrigin)
        {
            PendingHits = hits;
            Instigator = instigator;
            AttackOrigin = attackOrigin;
            FireRequested = true;
            ReloadRequested = false;
        }

        public void SetReloadRequest()
        {
            ReloadRequested = true;
            FireRequested = false;
        }

        public void ClearRequests()
        {
            FireRequested = false;
            ReloadRequested = false;
        }
    }
}
