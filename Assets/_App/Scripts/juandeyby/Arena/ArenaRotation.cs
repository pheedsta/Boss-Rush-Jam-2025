using System;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class ArenaRotation : MonoBehaviour
    {
        [SerializeField] private Ring ring;
        [SerializeField] private Animator animator;
        [SerializeField] private float rotationSpeed = 10f;
        private Vector3 _rotation;
        private bool _isRotating = true;

        private void Start()
        {
            ServiceLocator.Get<GameManager>().OnGamePhaseChanged += OnGamePhaseChanged;
        }

        private void OnGamePhaseChanged(GamePhase gamePhase)
        {
            Debug.Log("OnGamePhaseChanged " + gamePhase);
            if (gamePhase == GamePhase.Phase2)
            {
                if (ring == Ring.RingC)
                {
                    _isRotating = false;
                    animator.enabled = true;
                    animator.CrossFade("FallC", 2f);
                }
            }
            if (gamePhase == GamePhase.Phase3)
            {
                if (ring == Ring.RingB)
                {
                    _isRotating = false;
                    animator.enabled = true;
                    animator.CrossFade("FallB", 2f);
                }
            }
        }

        private void Update()
        {
            Debug.Log("Update");
            if (_isRotating)
            {
                Debug.Log("Rotate");
                Rotate();
            }
        }

        private void Rotate()
        {
            _rotation.y += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(_rotation);
        }
    }
    
    public enum Ring
    {
        RingC,
        RingB,
    }
}
