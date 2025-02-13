using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    [DefaultExecutionOrder(-100)]
    public class WormManager : MonoBehaviour
    {
        [SerializeField] private Boss.Boss boss;
        [SerializeField] private WormPoison wormPrefab;
        [SerializeField] private WormFire wormFirePrefab;
        private readonly Queue<Worm> _worms = new Queue<Worm>();
        private readonly int _maxWorms = 8;
        private Coroutine _spawnCoroutine;
        
        private void OnEnable()
        {
            ServiceLocator.Register<WormManager>(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Unregister<WormManager>();
        }

        private void Start()
        {
            for (var i = 0; i < _maxWorms / 2; i++)
            {
                var worm = Instantiate(wormPrefab, transform);
                _worms.Enqueue(worm);
            }
            
            for (var i = 0; i < _maxWorms / 2; i++)
            {
                var worm = Instantiate(wormFirePrefab, transform);
                _worms.Enqueue(worm);
            }
        }

        public Worm GetWorm()
        {
            if (_worms.Count == 0)
            {
                var worm = Instantiate(wormPrefab, transform);
                _worms.Enqueue(worm);
            }

            var wormToReturn = _worms.Dequeue();
            wormToReturn.MeshAgent.enabled = true; // Enable the mesh agent
            wormToReturn.Model.SetActive(true);
            return wormToReturn;
        }
        
        public void ReturnWorm(Worm worm)
        {
            worm.MeshAgent.Warp(Vector3.up * 2f);
            worm.MeshAgent.enabled = false; // Disable the mesh agent
            worm.Model.SetActive(false);
            _worms.Enqueue(worm);
            
            Debug.Log("Worm returned!");
            CheckWorms();
        }

        private void CheckWorms()
        {
            var children = transform.GetComponentsInChildren<Worm>(true);
            var activeWorms = children.Count(c => c.Model.activeSelf);
            if (activeWorms == 0)
            {
                // Debug.Log("All worms are dead!");
                // boss.SetState(new Boss.BossPortalSummonState());
                
                if (_spawnCoroutine != null)
                {
                    StopCoroutine(_spawnCoroutine);
                }
                Debug.Log("All worms are dead!");
                _spawnCoroutine = StartCoroutine(RespawnWorms());
            }
        }
        
        private IEnumerator RespawnWorms()
        {
            while (true)
            {
                yield return new WaitForSeconds(6f);
                boss.SetState(new Boss.BossPortalSummonState());
                break;
            }
        }
    }
}
