using UnityEngine;

namespace Infernum.FPS.Core.Animation
{
    public interface ISpriteSequenceTarget
    {
        bool IsValid { get; }
        bool Enabled { get; set; }
        Sprite Sprite { get; set; }
    }
}
