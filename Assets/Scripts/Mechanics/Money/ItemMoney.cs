using Jam.Effects;
using UnityEngine;
using UnityEngine.Pool;

namespace Jam.Mechanics.Money
{
    public class ItemMoney : MonoBehaviour
    {
        private ConstantOscillate _constantOscillate;
        private ConstantOscillate ConstantOscillate => _constantOscillate ??= GetComponent<ConstantOscillate>();
        
        public int MoneyValue { get; set; }
        
        private IObjectPool<ItemMoney> MoneyPool { get; set; }
        
        public void Build(IObjectPool<ItemMoney> moneyPool)
        {
            MoneyPool = moneyPool;
            gameObject.SetActive(false);
        }
        public void Activate(Mesh moneyMesh, int moneyValue, Vector3 spawnLocation)
        {
            transform.position = spawnLocation;
            ConstantOscillate.Activate(spawnLocation);
            MoneyValue = moneyValue;
            GetComponent<MeshFilter>().mesh = moneyMesh;
        }

        private void OnTriggerEnter(Collider other) =>
            MoneyPool.Release(this);
    }
}
