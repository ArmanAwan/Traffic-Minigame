using Jam.Effects;
using UnityEngine;

namespace Jam.Mechanics.Player
{
    public class PlayerVehicle : AbstractVehicle
    {
        [SerializeField]
        private ClickEffect _clickEffectPrefab;
        private ClickEffect ClickEffectPrefab => _clickEffectPrefab;
        
        
        private ClickEffect _clickEffect;
        private ClickEffect ClickEffect => _clickEffect ??= Instantiate(ClickEffectPrefab);
        
        [SerializeField]
        private float _rotationSpeed;
        private float RotationSpeed => _rotationSpeed;
        
        private Vector3 MoveLocation { get; set; }
        private Quaternion TargetRotation { get; set; }
        public void SetMoveLocation(Vector3 newMoveLocation)
        {
            MoveLocation = newMoveLocation;
            ClickEffect.Activate(newMoveLocation);
        }

        private void FixedUpdate()
        {
            if (!((CachedTransform.position - MoveLocation).magnitude > 0.3f)) return;
            Move();
            
            Vector3 direction = MoveLocation - CachedTransform.position;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            TargetRotation = Quaternion.Euler(0f,angle,0f);
            CachedTransform.rotation = Quaternion.RotateTowards(CachedTransform.rotation, TargetRotation, Time.deltaTime * RotationSpeed);
        }
    }
}
