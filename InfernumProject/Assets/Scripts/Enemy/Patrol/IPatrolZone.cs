using UnityEngine;

namespace Infernum.FPS.Enemy
{
    /// <summary>
    /// Зона патруля: набор точек в мировых координатах (OCP — можно заменить реализацию).
    /// </summary>
    public interface IPatrolZone
    {
        int WaypointCount { get; }
        Vector3 GetWaypoint(int index);
    }
}
