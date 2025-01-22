using System.Collections.Generic;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    [DefaultExecutionOrder(-100)]
    public class PortalManager : MonoBehaviour
    {
        [SerializeField] private Portal portalPrefab;
        private readonly Queue<Portal> _portals = new Queue<Portal>();
        private readonly int _maxPortals = 10;
        
        private void OnEnable()
        {
            ServiceLocator.Register<PortalManager>(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Unregister<PortalManager>();
        }
        
        private void Start()
        {
            for (int i = 0; i < _maxPortals; i++)
            {
                var portal = Instantiate(portalPrefab, transform);
                portal.gameObject.SetActive(false);
                _portals.Enqueue(portal);
            }
        }
        
        public Portal GetPortal()
        {
            if (_portals.Count == 0)
            {
                var portal = Instantiate(portalPrefab, transform);
                _portals.Enqueue(portal);
            }

            var portalToReturn = _portals.Dequeue();
            portalToReturn.gameObject.SetActive(true);
            return portalToReturn;
        }
        
        public void ReturnPortal(Portal portal)
        {
            portal.gameObject.SetActive(false);
            _portals.Enqueue(portal);
        }
    }
}
