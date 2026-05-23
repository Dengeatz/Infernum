using UnityEngine;

namespace Infernum.FPS.Core.Animation
{
    public sealed class SpriteRendererSpriteTarget : ISpriteSequenceTarget
    {
        private readonly SpriteRenderer _renderer;

        public SpriteRendererSpriteTarget(SpriteRenderer renderer)
        {
            _renderer = renderer;
        }

        public bool IsValid => _renderer != null;
        public bool Enabled { get => _renderer != null && _renderer.enabled; set { if (_renderer != null) _renderer.enabled = value; } }
        public Sprite Sprite { get => _renderer != null ? _renderer.sprite : null; set { if (_renderer != null) _renderer.sprite = value; } }
    }
}
