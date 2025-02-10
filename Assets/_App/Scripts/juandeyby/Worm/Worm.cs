using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class Worm : MonoBehaviour
    {
        [SerializeField] private GameObject model;
        public GameObject Model => model;
        [SerializeField] private Rigidbody rb;
        public Rigidbody Rb => rb;
        [SerializeField] private WormAnimator wormAnimator;
        public WormAnimator WormAnimator => wormAnimator;
        [SerializeField] private NavMeshAgent meshAgent;
        public NavMeshAgent MeshAgent => meshAgent;
        [SerializeField] private WormHealth wormHealth;
        public WormHealth WormHealth => wormHealth;
        private IWormState _currentState;

        public void SetState(IWormState state)
        {
            if (_currentState != null)
            {
                _currentState.Exit(this);
            }

            _currentState = state;
            _currentState.Enter(this);
        }

        private void Update()
        {
            if (_currentState != null)
            {
                _currentState.Update(this);
            }
            
            CheckFall();
        }
        
        private void CheckFall()
        {
            if (transform.position.y < -10)
            {
                ServiceLocator.Get<WormManager>().ReturnWorm(this);
            }
        }
    }
}