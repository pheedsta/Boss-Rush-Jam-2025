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
        [SerializeField] private PlayerGround playerGround;
        public PlayerGround PlayerGround => playerGround;
        
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
    }
}
