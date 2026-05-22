using System.Collections.Generic;
using UnityEngine;

namespace Infernum.FPS.Weapons.Animation
{
    public abstract class IdleAnimationConfig : ScriptableObject
    {
        [SerializeField] private List<Sprite> sprites = new();

        public IReadOnlyList<Sprite> Sprites => sprites;
    }
}
