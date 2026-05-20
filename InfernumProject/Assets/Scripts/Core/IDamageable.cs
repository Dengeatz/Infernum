using UnityEngine;

namespace Infernum.FPS.Core
{
    /// <summary>
    /// Контракт для объектов, по которым можно попасть оружием или получить урон.
    /// </summary>
    public interface IDamageable
    {
        bool IsAlive { get; }
        void TakeDamage(float damage, GameObject instigator);
    }
}
