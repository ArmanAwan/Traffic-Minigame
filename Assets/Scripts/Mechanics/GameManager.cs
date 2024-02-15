using Jam.Effects;
using Jam.Mechanics.Enemy;
using Jam.Mechanics.Player;
using Jam.Mechanics.Score;
using UnityEngine;

namespace Jam.Mechanics
{
    public class GameManager : MonoBehaviour //Considered an AbstractManager but saw no need
    {
        [SerializeField]
        private float _playTime;
        private float PlayTime => _playTime;
        public delegate void LevelStateDelegate();
        public event LevelStateDelegate LevelStartEvent;
        public event LevelStateDelegate LevelEndEvent;
        
        public const float PlayAreaInner = 9.5f;
        public const float PlayAreaOuter = 12.5f;
        public static bool GameIsRunning { get; private set; }
        public float LevelTimer { get; private set; }
        
        private SoundManager MainSoundManager { get; set; }
        private void Start() =>
            CriticalLoad();

        private void CriticalLoad()
        {
            MoneyManager moneyManager = gameObject.GetComponent<MoneyManager>();
            moneyManager.Activate(this);
            gameObject.GetComponent<PlayerManager>().Activate(this, moneyManager);
            gameObject.GetComponent<EnemyManager>().Activate(this);
            MainSoundManager = GetComponentInChildren<SoundManager>();
        }

        private void Update()
        {
            if(!GameIsRunning) return;
            LevelTimer -= Time.deltaTime;
            if (LevelTimer > 0) return;
            LevelEnd();
        }

        public void LevelStart()
        {
            LevelTimer = PlayTime;
            GameIsRunning = true;
            MainSoundManager.PlaySound(SoundManager.SoundType.LevelStart);
            LevelStartEvent?.Invoke();
        }

        private void LevelEnd()
        {
            GameIsRunning = false;
            MainSoundManager.PlaySound(SoundManager.SoundType.LevelEnd);
            LevelEndEvent?.Invoke();
        }
    }
}