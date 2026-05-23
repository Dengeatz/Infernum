namespace Infernum.FPS.Core.Animation
{
    public interface ISpriteAnimationService
    {
        bool IsPlaying { get; }
        void Tick(float deltaTime);
    }
}
