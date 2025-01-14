using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class Boss : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent meshAgent;
        private IBossState _currentState;
        
        [SerializeField] private GameObject vortexEffect;
        
        /// <summary>
        /// Set the current state of the boss
        /// </summary>
        /// <param name="newState"> The new state to set </param>
        public void SetState(IBossState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit(this);
            }
            _currentState = newState;
            _currentState.Enter(this);
        }
    
        private void Update()
        {
            if (_currentState != null)
            {
                _currentState.Update(this);
            }
        }

        /// <summary>
        /// Get the mesh agent of the boss
        /// </summary>
        /// <returns> The mesh agent of the boss </returns>
        public NavMeshAgent GetMeshAgent()
        {
            return meshAgent;
        }
        
        public void PlayVortexEffect()
        {
            vortexEffect.SetActive(true);
        }

        public void StopVortexEffect()
        {
            vortexEffect.SetActive(false);
        }
    }
}
