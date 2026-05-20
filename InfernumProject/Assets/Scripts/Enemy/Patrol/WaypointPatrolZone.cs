using Infernum.FPS.Enemy;
using UnityEngine;

namespace Infernum.FPS.Enemy.Patrol
{
    /// <summary>
    /// Простая зона из массива точек (пустой массив — патруль вокруг собственной позиции).
    /// </summary>
    public sealed class WaypointPatrolZone : MonoBehaviour, IPatrolZone
    {
        [SerializeField] private Transform[] waypoints;

        public int WaypointCount => waypoints != null ? waypoints.Length : 0;

        public Vector3 GetWaypoint(int index)
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                return transform.position;
            }

            int i = Mathf.Clamp(index, 0, waypoints.Length - 1);
            return waypoints[i] != null ? waypoints[i].position : transform.position;
        }
    }
}
