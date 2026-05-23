using System.Collections.Generic;
using UnityEngine;

namespace Infernum.FPS.Enemy.Animation
{
    public abstract class EnemyAttackAnimationConfig : ScriptableObject
    {
        [SerializeField] private List<Sprite> sprites = new();

        public IReadOnlyList<Sprite> Sprites => sprites;
    }
}
