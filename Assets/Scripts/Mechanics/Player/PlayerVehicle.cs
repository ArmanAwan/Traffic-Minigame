using System;
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

        private Action EnemyCollisionCallback { get; set; }
        private Action<int> MoneyCollisionCallback { get; set; }
        public void Activate(string newName, Action enemyHitCallback, Action<int> moneyCollisionCallback)
        {
            gameObject.name = newName;
            EnemyCollisionCallback = enemyHitCallback;
            MoneyCollisionCallback = moneyCollisionCallback;
        }
        public void SetMoveLocation(Vector3 newMoveLocation)
        {
            if (!GameManager.GameIsRunning) return;
            MoveLocation = newMoveLocation;
            ClickEffect.Activate(newMoveLocation);
        }

        private void FixedUpdate()
        {
            if (!GameManager.GameIsRunning || !((CachedTransform.position - MoveLocation).magnitude > 0.3f)) return;
            Move();
            
            Vector3 direction = MoveLocation - CachedTransform.position;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            TargetRotation = Quaternion.Euler(0f,angle,0f);
            CachedTransform.rotation = Quaternion.RotateTowards(CachedTransform.rotation, TargetRotation, Time.deltaTime * RotationSpeed);
        }
        private void OnTriggerEnter(Collider otherCollider)
        {
            switch (otherCollider.gameObject.layer)
            {
                case 7:
                    EnemyCollisionCallback?.Invoke();
                    break;
                case 8:
                {
                    ItemMoney money;
                    if(money = otherCollider.GetComponent<ItemMoney>())
                    {
                        MoneyCollisionCallback?.Invoke(money.MoneyValue);
                    }
                    break;
                }
            }
        }

        public void ResetTransform()
        {
            CachedTransform.position = Vector3.zero;
            CachedTransform.rotation = Quaternion.identity;
        }
    }
}
