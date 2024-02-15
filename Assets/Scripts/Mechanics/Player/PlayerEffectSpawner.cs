using Jam.Effects;
using UnityEngine;

namespace Jam.Mechanics.Player
{
    public class PlayerEffectSpawner : MonoBehaviour
    {
        [SerializeField]
        private EnvironmentEffect _coinCollectEffect;
        private EnvironmentEffect CoinCollectEffect => _coinCollectEffect;

        [SerializeField]
        private EnvironmentEffect _enemyDeathEffect;
        private EnvironmentEffect EnemyDeathEffect => _enemyDeathEffect;

        private Transform EffectHolderTransform { get; set; }
        
        public void Activate()
        {
            EffectHolderTransform = new GameObject("EffectHolder").transform;
        }

        //TODO Convert to object pooling (custom solution)
        //TODO Replace with array/serailised dictionary instead of multiple functions
        public void SpawnEnemyDeathEffect(Vector3 position) =>
            Instantiate(EnemyDeathEffect, position, Quaternion.identity, EffectHolderTransform);
        public void SpawnMoneyCollectEffect(Vector3 position) =>
            Instantiate(CoinCollectEffect, position, Quaternion.identity, EffectHolderTransform);
        
    }
}
