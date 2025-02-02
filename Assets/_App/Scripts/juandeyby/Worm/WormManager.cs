using System;
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
                worm.gameObject.SetActive(false);
                _worms.Enqueue(worm);
            }
            
            for (var i = 0; i < _maxWorms / 2; i++)
            {
                var worm = Instantiate(wormFirePrefab, transform);
                worm.gameObject.SetActive(false);
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
            wormToReturn.gameObject.SetActive(true);
            return wormToReturn;
        }
        
        public void ReturnWorm(Worm worm)
        {
            worm.gameObject.SetActive(false);
            _worms.Enqueue(worm);
            
            CheckWorms();
        }

        private void CheckWorms()
        {
            var children = transform.GetComponentsInChildren<Worm>(true);
            var activeWorms = children.Count(c => c.gameObject.activeSelf);
            if (activeWorms == 0)
            {
                boss.SetState(new Boss.BossPortalSummonState());
            }
        }
    }
}
