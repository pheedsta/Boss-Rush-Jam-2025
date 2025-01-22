using System;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int stepCount = 20;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private TargetMarker targetMarkerPrefab;
        private Vector3 _startVelocity;
        private Vector3 _startPosition;
        
        public void Config(Vector3 origin)
        {
            _startVelocity = (transform.position - origin).normalized * 10;
            rb.linearVelocity = _startVelocity;
            
            _startPosition = transform.position;
            
            DrawTarget();
            CheckLimit();
            
        }
        
        private void DrawTarget()
        {
            var time = 0f;
            var lastPosition = _startPosition;
            for (var i = 0; i < stepCount; i++)
            {
                var x = (_startVelocity.x * time) + (Physics.gravity.x / 2 * time * time);
                var y = (_startVelocity.y * time) + (Physics.gravity.y / 2 * time * time);
                var z = (_startVelocity.z * time) + (Physics.gravity.z / 2 * time * time);
                var currentPosition = new Vector3(x, y, z) + _startPosition;
                Debug.DrawLine(lastPosition, currentPosition, Color.red, 4f);
                
                if (Physics.Raycast(lastPosition, (currentPosition - lastPosition).normalized,
                        out var hit, (currentPosition - lastPosition).magnitude))
                {
                    var marker = Instantiate(targetMarkerPrefab, 
                        hit.point, Quaternion.LookRotation(hit.normal));
                    marker.transform.position += marker.transform.forward * 0.1f;
                    break;
                } 
                
                lastPosition = currentPosition;
                time += 0.25f;
            }
        }
        
        private void CheckLimit()
        {
            if (transform.position.y < -10)
            {
                ServiceLocator.Get<ProjectileManager>().ReturnProjectile(this);
            }
        }
    }
}
