namespace Infernum.FPS.Player
{
    public interface IPlayerMovement
    {
        bool Enabled { get; set; }
        void Tick(float deltaTime);
    }
}
