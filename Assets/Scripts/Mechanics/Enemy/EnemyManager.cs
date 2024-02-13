using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

namespace Jam.Mechanics.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private EnemyVehicle _enemyVehiclePrefab;
        private EnemyVehicle EnemyVehiclePrefab => _enemyVehiclePrefab;
        private Transform EnemyHolderTransform { get; set; }

        private const float PlayAreaEdge = 12f;
        private const float SpawnRate = 2f;
        private float SpawnTimer { get; set; }
        public void Activate()
        {
            EnemyHolderTransform = new GameObject
            {
                gameObject =
                {
                    name = "EnemyHolder"
                }
            }.transform;
        }

        private void Update()
        {
            if (!GameManager.GameIsRunning) return;
            SpawnTimer += Time.deltaTime;
            if(SpawnTimer < SpawnRate) return;
            SpawnTimer = 0;
            //TODO replace with pool
            EnemyVehicle enemy = Instantiate(EnemyVehiclePrefab, parent: EnemyHolderTransform);
            
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            float xPoint = Mathf.Cos(randomAngle) * PlayAreaEdge;
            float zPoint = Mathf.Sin(randomAngle) * PlayAreaEdge;
            Vector3 spawnPoint = new(xPoint, 0, zPoint);
            
            enemy.Activate(spawnPoint);
        }
    }
}
