using UnityEngine;
using UnityEngine.UI;

namespace Infernum.FPS.Core.Animation
{
    public sealed class ImageSpriteTarget : ISpriteSequenceTarget
    {
        private readonly Image _image;

        public ImageSpriteTarget(Image image)
        {
            _image = image;
        }

        public bool IsValid => _image != null;
        public bool Enabled { get => _image != null && _image.enabled; set { if (_image != null) _image.enabled = value; } }
        public Sprite Sprite { get => _image != null ? _image.sprite : null; set { if (_image != null) _image.sprite = value; } }
    }
}
