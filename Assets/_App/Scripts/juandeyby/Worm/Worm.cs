using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby
{
    public class Worm : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _meshAgent;
        public NavMeshAgent MeshAgent => _meshAgent;
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
        }
    }
}