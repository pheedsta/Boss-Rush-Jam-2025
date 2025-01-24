using System;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float gravityMultiplier = 25f;
        [SerializeField] private int stepCount = 100;
        [SerializeField] private Rigidbody rb;
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

        private void FixedUpdate()
        {
            if (rb.linearVelocity.y < 0)
            {
                var downwardVelocity = rb.linearVelocity;
                downwardVelocity.y -= gravityMultiplier * Time.fixedDeltaTime;
                rb.linearVelocity = downwardVelocity;
            }
        }

        private void DrawTarget()
        {
            var time = 0f;
            var lastPosition = _startPosition;
            var lastDirection = _startVelocity.normalized;
            for (var i = 0; i < stepCount; i++)
            {
                var x = (_startVelocity.x * time) + (Physics.gravity.x / 2 * time * time);
                var y = (_startVelocity.y * time) + (Physics.gravity.y / 2 * time * time);
                if (lastDirection.y < 0)
                {
                    _startVelocity.y -= gravityMultiplier * 0.01f;
                    y = (_startVelocity.y * time) + (Physics.gravity.y / 2 * time * time);
                }
                var z = (_startVelocity.z * time) + (Physics.gravity.z / 2 * time * time);
                var currentPosition = new Vector3(x, y, z) + _startPosition;
                Debug.DrawLine(lastPosition, currentPosition, Color.red, 4f);
                
                lastDirection = (currentPosition - lastPosition).normalized;
                
                if (Physics.Raycast(lastPosition, (currentPosition - lastPosition).normalized,
                        out var hit, (currentPosition - lastPosition).magnitude))
                {
                    var targetMarker = ServiceLocator.Get<TargetMarkerManager>().GetTargetMarker();
                    targetMarker.transform.position = hit.point;
                    targetMarker.transform.position += hit.normal * 0.1f;
                    targetMarker.transform.rotation = Quaternion.LookRotation(hit.normal);
                    break;
                }

                lastPosition = currentPosition;
                time += 0.05f;
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
