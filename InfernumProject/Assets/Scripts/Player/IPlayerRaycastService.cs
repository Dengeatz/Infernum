using System.Collections.Generic;
using UnityEngine;

namespace Infernum.FPS.Player
{
    public interface IPlayerRaycastService
    {
        IReadOnlyList<RaycastHit> Hits { get; }
        void Tick();
    }
}
