using System.Collections;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class TargetMarker : MonoBehaviour
    {
        [SerializeField] private float timeToDestroy = 4f;
        private Coroutine _destroyCoroutine;
        
        private void OnEnable()
        {
            if (_destroyCoroutine != null)
            {
                StopCoroutine(_destroyCoroutine);
            }
            _destroyCoroutine = StartCoroutine(DelayedDestroy(timeToDestroy));
        }
        
        private IEnumerator DelayedDestroy(float delay)
        {
            yield return new WaitForSeconds(delay);
            ServiceLocator.Get<TargetMarkerManager>().ReturnTargetMarker(this);
        }
    }
}
