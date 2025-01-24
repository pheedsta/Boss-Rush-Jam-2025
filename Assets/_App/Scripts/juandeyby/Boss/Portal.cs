using System;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private GameObject wormPrefab;
        [SerializeField] private GameObject ghostPrefab;
        
        private void Start()
        {
            var enemy = UnityEngine.Random.Range(0, 2) == 0 ? wormPrefab : ghostPrefab;
            Instantiate(enemy, transform.position, Quaternion.identity);
            Destroy(gameObject, 2f);
        }
    }
}
