using System.Collections.Generic;
using Infernum.FPS.Enemy.Animation;
using UnityEngine;

namespace Infernum.FPS.Enemy.Config
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Infernum/Enemy/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private EnemyAnimationConfig animationConfig;

        public EnemyAnimationConfig AnimationConfig => animationConfig;

        public IReadOnlyList<Sprite> GetIdleSprites() => GetSprites(animationConfig?.Idle);
        public IReadOnlyList<Sprite> GetMoveSprites() => GetSprites(animationConfig?.Move);
        public IReadOnlyList<Sprite> GetAttackSprites() => GetSprites(animationConfig?.Attack);
        public IReadOnlyList<Sprite> GetDeadSprites() => GetSprites(animationConfig?.Dead);

        public Sprite GetIdlePreviewSprite()
        {
            IReadOnlyList<Sprite> sprites = GetIdleSprites();
            return sprites.Count > 0 ? sprites[0] : null;
        }

        private static IReadOnlyList<Sprite> GetSprites(EnemyIdleAnimationConfig config)
        {
            return config != null ? config.Sprites : System.Array.Empty<Sprite>();
        }

        private static IReadOnlyList<Sprite> GetSprites(EnemyMoveAnimationConfig config)
        {
            return config != null ? config.Sprites : System.Array.Empty<Sprite>();
        }

        private static IReadOnlyList<Sprite> GetSprites(EnemyAttackAnimationConfig config)
        {
            return config != null ? config.Sprites : System.Array.Empty<Sprite>();
        }

        private static IReadOnlyList<Sprite> GetSprites(EnemyDeadAnimationConfig config)
        {
            return config != null ? config.Sprites : System.Array.Empty<Sprite>();
        }
    }
}
