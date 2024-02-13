using UnityEngine;

namespace Jam.Mechanics.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerVehicle _playerVehiclePrefab;
        private PlayerVehicle PlayerVehiclePrefab => _playerVehiclePrefab;
        
        private PlayerVehicle Player { get; set; }
        
        private Camera _cachedCamera;
        private Camera CachedCamera => _cachedCamera ??= Camera.main;

        private Plane FloorPlane { get; set; }

        public void Activate()
        {
            PlayerVehicle playerVehicle = Instantiate(PlayerVehiclePrefab, Vector3.zero, Quaternion.identity);
            playerVehicle.name = "Player";
            Player = playerVehicle;
            FloorPlane = new Plane(Vector3.up, Vector3.zero);
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
            if(clickLocation.magnitude > GameManager.PlayAreaRadius)
            {
                clickLocation = clickLocation.normalized * GameManager.PlayAreaRadius;
            }
            Player.SetMoveLocation(clickLocation);
        }
    }
}
