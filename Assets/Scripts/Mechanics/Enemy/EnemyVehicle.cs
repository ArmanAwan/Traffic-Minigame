using UnityEngine;

namespace Jam.Mechanics.Enemy
{
    public class EnemyVehicle : AbstractVehicle
    {
        [SerializeField]
        private float _minSpeed;
        private float MinSpeed => _minSpeed;
        [SerializeField]
        private float _maxSpeed;
        private float MaxSpeed => _maxSpeed;
        
        private void FixedUpdate()
        {
            Move();
        }

        public void Activate(Vector3 spawnPoint)
        {
            MoveSpeed = Random.Range(MinSpeed, MaxSpeed);
            CachedTransform.position = spawnPoint;
            float angleToCentre = Mathf.Atan2(-spawnPoint.x, -spawnPoint.z);
            float randomAngle = angleToCentre + Random.Range(-Mathf.PI / 4, Mathf.PI / 4);
            transform.rotation = Quaternion.Euler(0,Mathf.Rad2Deg * randomAngle,0);
        }
    }
}
