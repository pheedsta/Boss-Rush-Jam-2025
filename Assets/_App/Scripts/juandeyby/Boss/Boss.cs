using System;
using _App.Scripts.juandeyby.UI;
using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class Boss : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent meshAgent;
        private IBossState _currentState;
        
        // VFX
        [SerializeField] private GameObject vortexEffect;
        
        // SFX
        [SerializeField] private AK.Wwise.Event vortexPullSoundStart;
        [SerializeField] private AK.Wwise.Event vortexPullSoundStop;
        
        private UIHealthBar _healthBar;

        private void Start()
        {
            _healthBar = UIServiceLocator.Get<UIHealthCanvas>().GetHealthBar();
            _healthBar.SetUnit(transform, new Vector3(0, 2f, 0));
        }

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
        
        /// <summary>
        /// Play the vortex effect
        /// </summary>
        public void PlayVortexEffect()
        {
            vortexPullSoundStart.Post(gameObject);
            vortexEffect.SetActive(true);
        }

        /// <summary>
        /// Stop the vortex effect
        /// </summary>
        public void StopVortexEffect()
        {
            vortexPullSoundStart.Stop(gameObject);
            vortexPullSoundStop.Post(gameObject);
            vortexEffect.SetActive(false);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 7f);
        }

        /// <summary>
        /// Play the sweeping strike effect
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void PlaySweepingStrikeEffect()
        {
            Debug.LogWarningFormat("<color=red>PlaySweepingStrikeEffect not implemented</color>");
        }
        
        /// <summary>
        /// Stop the sweeping strike effect
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void StopSweepingStrikeEffect()
        {
            Debug.LogWarningFormat("<color=red>StopSweepingStrikeEffect not implemented</color>");
        }

        private void OnDestroy()
        {
            UIServiceLocator.Get<UIHealthCanvas>().ReturnHealthBar(_healthBar);
        }
    }
}
