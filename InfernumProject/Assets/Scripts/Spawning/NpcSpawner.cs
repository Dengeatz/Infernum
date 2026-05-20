using Infernum.FPS.Enemy;
using Infernum.FPS.Enemy.Patrol;
using Infernum.FPS.Player;
using UnityEngine;

namespace Infernum.FPS.Spawning
{
    /// <summary>
    /// Контракт сервиса спавна NPC (DIP — логика может подменяться тестовой реализацией).
    /// </summary>
    public interface INpcSpawner
    {
        void Spawn(int enemyCount);
    }

    /// <summary>
    /// Спавн врагов в объёме <see cref="BoxCollider"/> с привязкой к игроку и зоне патруля.
    /// </summary>
    public sealed class NpcSpawner : MonoBehaviour, INpcSpawner
    {
        [SerializeField] private Enemy.Enemy enemyPrefab;
        [SerializeField] private Player.Player player;
        [SerializeField] private WaypointPatrolZone patrolZone;
        [SerializeField] private BoxCollider spawnVolume;
        [SerializeField] private Transform spawnParent;
        [SerializeField] private int maxSpawnPerCall = 64;

        private void Awake()
        {
            if (spawnVolume == null)
            {
                spawnVolume = GetComponent<BoxCollider>();
            }
        }

        public void Spawn(int enemyCount)
        {
            if (enemyPrefab == null)
            {
                Debug.LogWarning("NpcSpawner: не назначен префаб врага.");
                return;
            }

            int count = Mathf.Clamp(enemyCount, 0, maxSpawnPerCall);
            IPlayer playerFacade = player;
            IPatrolZone zone = patrolZone;

            for (int i = 0; i < count; i++)
            {
                Vector3 position = GetRandomPointInBox(spawnVolume);
                Enemy.Enemy instance = Instantiate(enemyPrefab, position, Quaternion.identity, spawnParent);
                instance.Configure(playerFacade, zone);
            }
        }

        private static Vector3 GetRandomPointInBox(BoxCollider box)
        {
            if (box == null)
            {
                return Vector3.zero;
            }

            Vector3 half = box.size * 0.5f;
            Vector3 local = new Vector3(
                Random.Range(-half.x, half.x),
                Random.Range(-half.y, half.y),
                Random.Range(-half.z, half.z));
            return box.transform.TransformPoint(box.center + local);
        }
    }
}
