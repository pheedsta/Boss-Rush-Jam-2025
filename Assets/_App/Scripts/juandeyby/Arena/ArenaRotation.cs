using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class ArenaRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 10f;
        private Vector3 _rotation;

        private void Update()
        {
            _rotation.y += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(_rotation);
        }
    }
}
