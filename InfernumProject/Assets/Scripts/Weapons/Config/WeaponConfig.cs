using System.Collections.Generic;
using Infernum.FPS.Weapons.Animation;
using UnityEngine;

namespace Infernum.FPS.Weapons.Config
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Infernum/Weapons/Weapon Config")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private float damage = 25f;
        [SerializeField] private float damageDistance = 80f;
        [SerializeField] private WeaponAnimationConfig animationConfig;
        [SerializeField] private GameObject worldPickupPrefab;

        public float Damage => damage;
        public float DamageDistance => damageDistance;
        public WeaponAnimationConfig AnimationConfig => animationConfig;
        public GameObject WorldPickupPrefab => worldPickupPrefab;

        public IReadOnlyList<Sprite> GetIdleSprites() => GetSprites(animationConfig?.Idle);
        public IReadOnlyList<Sprite> GetFireSprites() => GetSprites(animationConfig?.Fire);
        public IReadOnlyList<Sprite> GetReloadSprites() => GetSprites(animationConfig?.Reload);

        public Sprite GetIdlePreviewSprite()
        {
            IReadOnlyList<Sprite> sprites = GetIdleSprites();
            return sprites.Count > 0 ? sprites[0] : null;
        }

        private static IReadOnlyList<Sprite> GetSprites(IdleAnimationConfig config)
        {
            return config != null ? config.Sprites : System.Array.Empty<Sprite>();
        }

        private static IReadOnlyList<Sprite> GetSprites(FireAnimationConfig config)
        {
            return config != null ? config.Sprites : System.Array.Empty<Sprite>();
        }

        private static IReadOnlyList<Sprite> GetSprites(ReloadAnimationConfig config)
        {
            return config != null ? config.Sprites : System.Array.Empty<Sprite>();
        }
    }
}
