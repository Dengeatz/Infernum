using UnityEngine;

namespace Infernum.FPS.Weapons.Animation
{
    [CreateAssetMenu(fileName = "WeaponAnimationConfig", menuName = "Infernum/Weapons/Weapon Animation Config")]
    public class WeaponAnimationConfig : ScriptableObject
    {
        [Header("Конфиги кадров")]
        [SerializeField] private ReloadAnimationConfig reload;
        [SerializeField] private FireAnimationConfig fire;
        [SerializeField] private IdleAnimationConfig idle;

        [Header("Интервалы кадров (сек, в духе Doom)")]
        [SerializeField] private float idleFrameInterval = 0.35f;
        [SerializeField] private float fireFrameInterval = 0.2f;
        [SerializeField] private float reloadFrameInterval = 0.45f;

        public ReloadAnimationConfig Reload => reload;
        public FireAnimationConfig Fire => fire;
        public IdleAnimationConfig Idle => idle;

        public float IdleFrameInterval => idleFrameInterval;
        public float FireFrameInterval => fireFrameInterval;
        public float ReloadFrameInterval => reloadFrameInterval;
    }
}
