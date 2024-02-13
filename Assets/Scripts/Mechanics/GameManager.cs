using Jam.Mechanics.Enemy;
using Jam.Mechanics.Player;
using UnityEngine;

namespace Jam.Mechanics
{
    public class GameManager : MonoBehaviour
    {
        public const float PlayAreaRadius = 9.5f;
        public static bool GameIsRunning { get; set; } = true; //TODO Remove default
        
        private void Start() =>
            CriticalLoad();

        private void CriticalLoad()
        {
            gameObject.GetComponent<PlayerManager>().Activate();
            gameObject.GetComponent<EnemyManager>().Activate();
        }
    }
}