using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Jam
{
    public class ItemMoney : MonoBehaviour
    {
        public int MoneyValue { get; set; }
        
        private IObjectPool<ItemMoney> MoneyPool { get; set; }
        
        public void Build(IObjectPool<ItemMoney> moneyPool)
        {
            MoneyPool = moneyPool;
            gameObject.SetActive(false);
        }
        public void Activate(Mesh moneyMesh, int moneyValue, Vector3 spawnLocation)
        {
            transform.localPosition = spawnLocation;
            MoneyValue = moneyValue;
            GetComponent<MeshFilter>().mesh = moneyMesh;
        }

        private void OnTriggerEnter(Collider other)
        {
            CollectEffect();
        }

        private void CollectEffect()
        {
            MoneyPool.Release(this);
            //TODO: Make effect
        }
    }
}
