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
            
            var worm = ServiceLocator.Get<WormManager>().GetWorm();
            worm.transform.position = transform.position;
            yield return new WaitForSeconds(2f);
            ServiceLocator.Get<PortalManager>().ReturnPortal(this);
        }
    }
}
