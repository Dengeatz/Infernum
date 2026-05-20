namespace Infernum.FPS.Player
{
    public interface IPlayerCamera
    {
        bool Enabled { get; set; }
        void Tick(float deltaTime);
    }
}
