using UnityEngine;

namespace Jam.Mechanics
{
    public abstract class AbstractVehicle : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed;
        protected float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        private Transform _cachedTransform;
        protected Transform CachedTransform => _cachedTransform ??= transform;

        protected void Move()
        {
            CachedTransform.position += CachedTransform.forward * (MoveSpeed * Time.deltaTime);
        }
    }
}
