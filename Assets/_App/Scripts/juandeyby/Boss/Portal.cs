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
        
        private void OnEnable()
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
            }
            _spawnCoroutine = StartCoroutine(DelayedSpawn(1f));
        }
        
        private IEnumerator DelayedSpawn(float delay)
        {
            yield return new WaitForSeconds(delay);
            _spawnCoroutine = null;
            
            ServiceLocator.Get<MusicManager>().PlayPortalSummon();
            var worm = ServiceLocator.Get<WormManager>().GetWorm();
            worm.transform.position = transform.position;
            worm.SetState(new WormSpawnState());
            yield return new WaitForSeconds(2.5f);
            ServiceLocator.Get<MusicManager>().StopPortalSummon();
            ServiceLocator.Get<PortalManager>().ReturnPortal(this);
        }
    }
}
