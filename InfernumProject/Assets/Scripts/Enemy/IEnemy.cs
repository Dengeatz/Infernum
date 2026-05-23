using Infernum.FPS.Core;
using Infernum.FPS.Player;
using UnityEngine;

namespace Infernum.FPS.Enemy
{
    /// <summary>
    /// Внешний контракт врага (фасад для AI, спавнера, квестов).
    /// </summary>
    public interface IEnemy : IDamageable
    {
        Transform Transform { get; }
        bool IsAlive { get; }

        void Configure(IPlayer player, IPatrolZone patrolZone);
    }
}
