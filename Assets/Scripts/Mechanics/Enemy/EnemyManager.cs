using UnityEngine;
using UnityEngine.Pool;

namespace Jam.Mechanics.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private EnemyVehicle _enemyVehiclePrefab;
        private EnemyVehicle EnemyVehiclePrefab => _enemyVehiclePrefab;
        private Transform EnemyHolderTransform { get; set; }

        [SerializeField]
        private float _spawnRate;
        private float SpawnRate => _spawnRate;
        private float CurrentTimer { get; set; }

        private IObjectPool<EnemyVehicle> _enemyPool;
        private IObjectPool<EnemyVehicle> EnemyPool => _enemyPool ??= new ObjectPool<EnemyVehicle>(CreatePooledItem, GetPooledItem, ReleasePooledItem, DestroyPooledItem);

        public void Activate(GameManager gameManager)
        {
            EnemyHolderTransform = new GameObject("EnemyHolder").transform;
            gameManager.LevelEndEvent += LevelEnd;
        }

        private void Update()
        {
            if (!GameManager.GameIsRunning) return;

            CurrentTimer -= Time.deltaTime;
            if (CurrentTimer > 0) return;
            CurrentTimer = SpawnRate * Random.Range(1f, 1.5f);
            EnemyVehicle enemy = EnemyPool.Get();

            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            float xPoint = Mathf.Cos(randomAngle) * GameManager.PlayAreaOuter;
            float zPoint = Mathf.Sin(randomAngle) * GameManager.PlayAreaOuter;
            Vector3 spawnPoint = new(xPoint, 0, zPoint);

            enemy.Activate(spawnPoint);
        }
        
        private void LevelEnd()
        {
            foreach (Transform enemyTransform in EnemyHolderTransform)
            {
                if (enemyTransform.gameObject.activeSelf)
                {
                    EnemyPool.Release(enemyTransform.gameObject.GetComponent<EnemyVehicle>());
                }
            }
        }

        private EnemyVehicle CreatePooledItem()
        {
            EnemyVehicle enemy = Instantiate(EnemyVehiclePrefab, parent: EnemyHolderTransform);
            enemy.Build(EnemyPool);
            return enemy;
        }
        private void GetPooledItem(EnemyVehicle vehicle) =>
            vehicle.gameObject.SetActive(true);
        private void ReleasePooledItem(EnemyVehicle vehicle) =>
            vehicle.gameObject.SetActive(false);
        private void DestroyPooledItem(EnemyVehicle vehicle) =>
            Destroy(vehicle.gameObject);
    }
}