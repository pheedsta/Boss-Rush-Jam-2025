using System;
using UnityEngine;
using UnityEngine.Serialization;


namespace _App.Scripts.juandeyby
{
    [DefaultExecutionOrder(-100)]
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
        
        [SerializeField] private PlayerCollection playerCollection;
        public PlayerCollection PlayerCollection => playerCollection;
        
        [Header("Player Settings")]
        [SerializeField] Transform spawnPoint;
        [SerializeField] private Rigidbody rb;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (transform.position.y < -10)
            {
                transform.position = spawnPoint.position;
                transform.rotation = spawnPoint.rotation;
                rb.linearVelocity = Vector3.zero;
            }
        }
    }
}
