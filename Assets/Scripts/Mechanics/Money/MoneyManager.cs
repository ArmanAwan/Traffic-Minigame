using Jam.Mechanics.Money;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Jam.Mechanics.Score
{
    public class MoneyManager : MonoBehaviour
    {
        [SerializeField]
        private ItemMoney _moneyPrefab;
        private ItemMoney MoneyPrefab => _moneyPrefab;

        //consider serializable dictionary
        [SerializeField]
        private Mesh[] _moneyMeshes;
        private Mesh[] MoneyMeshes => _moneyMeshes;

        [SerializeField]
        private int[] _moneyValues;
        private int[] MoneyValues => _moneyValues;


        [SerializeField]
        private float _spawnRate;
        private float SpawnRate => _spawnRate;

        private float CurrentTimer { get; set; }
        
        private IObjectPool<ItemMoney> _moneyPool;
        private IObjectPool<ItemMoney> MoneyPool => _moneyPool ??= new ObjectPool<ItemMoney>(CreatePooledItem, GetPooledItem, ReleasePooledItem, DestroyPooledItem);
        private Transform MoneyHolderTransform { get; set; }

        //The higher value, the higher the chance of low value money spawns
        private const float LowerValueSkew = 3.2f;
        private float MaxMoneyIndexWithSkew { get; set; }

        public delegate void UpdateScoreDelegate(int newScore);
        public static event UpdateScoreDelegate UpdateScoreEvent;
        
        private int _currentScore;
        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = Mathf.Clamp(value, 0, int.MaxValue);
                UpdateScoreEvent?.Invoke(_currentScore);
            }
        }
        public static int HighScore
        {
            get => PlayerPrefs.GetInt("HighScore", 0);
            set
            {
                if (value > HighScore)
                    PlayerPrefs.SetInt("HighScore", value);
            }
        }


        public void Activate(GameManager gameManager)
        {
            MoneyHolderTransform = new GameObject("MoneyHolder").transform;
            MaxMoneyIndexWithSkew = Mathf.Pow(MoneyMeshes.Length, LowerValueSkew);
            gameManager.LevelStartEvent += () => { CurrentScore = 0; };
            gameManager.LevelEndEvent += LevelEnd;
        }

        private void Update()
        {
            if (!GameManager.GameIsRunning) return;

            CurrentTimer -= Time.deltaTime;
            if (CurrentTimer > 0) return;
            CurrentTimer = SpawnRate * Random.Range(1f, 1.5f);
            int itemIndex = Mathf.RoundToInt(MoneyMeshes.Length - Mathf.Pow(Random.Range(0, MaxMoneyIndexWithSkew), 1f / LowerValueSkew));
            Vector2 randomLocation = Random.insideUnitCircle * GameManager.PlayAreaInner;
            Vector3 spawnLocation = new(randomLocation.x, 0.5f, randomLocation.y); //Spawns off the ground to account for oscillating movement
            ItemMoney money = MoneyPool.Get();
            money.Activate(MoneyMeshes[itemIndex], MoneyValues[itemIndex], spawnLocation);
        }

        private void LevelEnd()
        {
            HighScore = CurrentScore;
            
            foreach (Transform enemyTransform in MoneyHolderTransform)
            {
                if (enemyTransform.gameObject.activeSelf)
                {
                    MoneyPool.Release(enemyTransform.gameObject.GetComponent<ItemMoney>());
                }
            }
        }
        private ItemMoney CreatePooledItem()
        {
            ItemMoney money = Instantiate(MoneyPrefab, parent: MoneyHolderTransform);
            money.Build(MoneyPool);
            return money;
        }
        private void GetPooledItem(ItemMoney vehicle) =>
            vehicle.gameObject.SetActive(true);
        private void ReleasePooledItem(ItemMoney vehicle) =>
            vehicle.gameObject.SetActive(false);
        private void DestroyPooledItem(ItemMoney vehicle) =>
            Destroy(vehicle.gameObject);
    }
}