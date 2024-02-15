using Jam.Mechanics.Score;
using UnityEngine;

namespace Jam.Mechanics.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerVehicle _playerVehiclePrefab;
        private PlayerVehicle PlayerVehiclePrefab => _playerVehiclePrefab;

        private Camera _cachedCamera;
        private Camera CachedCamera => _cachedCamera ??= Camera.main;

        private PlayerVehicle Player { get; set; }
        private MoneyManager MoneyManager { get; set; }

        private Plane FloorPlane { get; set; }

        public void Activate(GameManager gameManager, MoneyManager moneyManager)
        {
            PlayerVehicle playerVehicle = Instantiate(PlayerVehiclePrefab, Vector3.zero, Quaternion.identity);
            playerVehicle.Activate("Player", RegisterEnemyCollision, RegisterMoneyCollision);
            Player = playerVehicle;

            FloorPlane = new Plane(Vector3.up, Vector3.zero);

            MoneyManager = moneyManager;
            gameManager.LevelStartEvent += () => { Player.ResetTransform(); };
        }

        private void Update()
        {
            if (!GameManager.GameIsRunning || !Input.GetMouseButtonDown(0)) return;
            Ray ray = CachedCamera.ScreenPointToRay(Input.mousePosition);
            if (FloorPlane.Raycast(ray, out float hitDistance))
            {
                RegisterClick(ray.GetPoint(hitDistance));
            }
        }

        private void RegisterClick(Vector3 clickLocation)
        {
            if (clickLocation.magnitude > GameManager.PlayAreaInner)
            {
                clickLocation = clickLocation.normalized * GameManager.PlayAreaInner;
            }

            Player.SetMoveLocation(clickLocation);
        }

        private void RegisterEnemyCollision() =>
            MoneyManager.CurrentScore -= 10000;

        private void RegisterMoneyCollision(int money) =>
            MoneyManager.CurrentScore += money;
    }
}