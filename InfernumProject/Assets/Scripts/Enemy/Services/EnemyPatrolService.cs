using Infernum.FPS.Enemy;
using UnityEngine;

namespace Infernum.FPS.Enemy.Services
{
    /// <summary>
    /// Перемещение между точками патруля в пределах зоны.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class EnemyPatrolService : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float reachDistance = 0.75f;
        [SerializeField] private float wanderRadius = 6f;

        private CharacterController _controller;
        private IPatrolZone _zone;
        private int _waypointIndex;
        private Vector3 _currentGoal;
        private bool _hasGoal;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void BindZone(IPatrolZone zone)
        {
            _zone = zone;
            _waypointIndex = 0;
            _hasGoal = false;
        }

        public void Tick(float deltaTime, Vector3 spawnOrigin)
        {
            if (_controller == null || !_controller.enabled)
            {
                return;
            }

            if (!_hasGoal)
            {
                PickNextGoal(spawnOrigin);
            }

            Vector3 flatSelf = transform.position;
            Vector3 flatGoal = _currentGoal;
            flatSelf.y = 0f;
            flatGoal.y = 0f;

            if ((flatGoal - flatSelf).sqrMagnitude <= reachDistance * reachDistance)
            {
                PickNextGoal(spawnOrigin);
            }

            Vector3 dir = (_currentGoal - transform.position);
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f)
            {
                return;
            }

            dir.Normalize();
            Vector3 motion = dir * moveSpeed;
            if (!_controller.isGrounded)
            {
                motion.y = -9.81f;
            }

            _controller.Move(motion * deltaTime);
        }

        public void StopHorizontal()
        {
            _hasGoal = false;
        }

        private void PickNextGoal(Vector3 spawnOrigin)
        {
            if (_zone != null && _zone.WaypointCount > 0)
            {
                _currentGoal = _zone.GetWaypoint(_waypointIndex);
                _waypointIndex = (_waypointIndex + 1) % _zone.WaypointCount;
            }
            else
            {
                Vector2 rnd = Random.insideUnitCircle * wanderRadius;
                _currentGoal = spawnOrigin + new Vector3(rnd.x, 0f, rnd.y);
            }

            _hasGoal = true;
        }
    }
}
