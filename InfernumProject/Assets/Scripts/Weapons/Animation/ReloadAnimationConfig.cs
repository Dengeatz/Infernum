using System.Collections.Generic;
using UnityEngine;

namespace Infernum.FPS.Weapons.Animation
{
    public abstract class ReloadAnimationConfig : ScriptableObject
    {
        [SerializeField] private List<Sprite> sprites = new();

        public IReadOnlyList<Sprite> Sprites => sprites;
    }
}
