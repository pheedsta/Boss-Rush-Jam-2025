using System;
using System.Collections.Generic;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    [DefaultExecutionOrder(-100)]
    public class TargetMarkerManager : MonoBehaviour
    {
        [SerializeField] private TargetMarker targetMarkerPrefab;
        private readonly Queue<TargetMarker> _targetMarkers = new Queue<TargetMarker>();
        private readonly int _maxTargetMarkers = 20;

        private void OnEnable()
        {
            ServiceLocator.Register<TargetMarkerManager>(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Unregister<TargetMarkerManager>();
        }

        private void Start()
        {
            for (var i = 0; i < _maxTargetMarkers; i++)
            {
                var targetMarker = Instantiate(targetMarkerPrefab, transform);
                targetMarker.gameObject.SetActive(false);
                _targetMarkers.Enqueue(targetMarker);
            }
        }
        
        public TargetMarker GetTargetMarker()
        {
            if (_targetMarkers.Count == 0)
            {
                var targetMarker = Instantiate(targetMarkerPrefab, transform);
                _targetMarkers.Enqueue(targetMarker);
            }

            var targetMarkerToReturn = _targetMarkers.Dequeue();
            targetMarkerToReturn.gameObject.SetActive(true);
            return targetMarkerToReturn;
        }

        public void ReturnTargetMarker(TargetMarker targetMarker)
        {
            targetMarker.gameObject.SetActive(false);
            _targetMarkers.Enqueue(targetMarker);
        }
    }
}
