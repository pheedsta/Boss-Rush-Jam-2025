using System;
using System.Collections;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] private GameObject wormPrefab;
        [SerializeField] private GameObject ghostPrefab;
        private Coroutine _spawnCoroutine;
        
        private void Start()
        {
            _spawnCoroutine = StartCoroutine(DelayedSpawn(1f));
        }
        
        private IEnumerator DelayedSpawn(float delay)
        {
            yield return new WaitForSeconds(delay);
            _spawnCoroutine = null;
            
            var enemy = UnityEngine.Random.Range(0, 2) == 0 ? wormPrefab : ghostPrefab;
            Instantiate(enemy, transform.position, Quaternion.identity);
            Destroy(gameObject, 2f);
        }
    }
}
