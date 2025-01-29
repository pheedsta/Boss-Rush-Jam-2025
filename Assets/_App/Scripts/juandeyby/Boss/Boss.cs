using System;
using _App.Scripts.juandeyby.UI;
using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class Boss : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent meshAgent;
        public NavMeshAgent MeshAgent => meshAgent;
        private IBossState _currentState;
        
        // VFX
        [SerializeField] private GameObject vortexEffect;
        
        // SFX
        [SerializeField] private AK.Wwise.Event vortexPullSoundStart;
        [SerializeField] private AK.Wwise.Event vortexPullSoundStop;
        
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

        /// <summary>
        /// Play the sweeping strike effect
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void PlaySweepingStrikeEffect()
        {
            Debug.LogWarningFormat("<color=cyan>PlaySweepingStrikeEffect not implemented</color>");
        }
        
        /// <summary>
        /// Stop the sweeping strike effect
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void StopSweepingStrikeEffect()
        {
            Debug.LogFormat("<color=cyan>StopSweepingStrikeEffect not implemented</color>");
        }

        // private void OnDestroy()
        // {
        //     UIServiceLocator.Get<UIHealthCanvas>().ReturnHealthBar(_healthBar);
        // }

        /// <summary>
        /// Play the aerial barrage effect
        /// </summary>
        public void PlayAerialBarrageEffect()
        {
            Debug.LogFormat("<color=cyan>PlayAerialBarrageEffect not implemented</color>");
        }
        
        /// <summary>
        /// Stop the aerial barrage effect
        /// </summary>
        public void StopAerialBarrageEffect()
        {
            Debug.LogFormat("<color=cyan>StopAerialBarrageEffect not implemented</color>");
        }

        /// <summary>
        /// Play the portal summon effect
        /// </summary>
        public void PlayPortalSummonEffect()
        {
            Debug.LogFormat("<color=cyan>PlayPortalSummonEffect not implemented</color>");
        }

        /// <summary>
        /// Stop the portal summon effect
        /// </summary>
        public void StopPortalSummonEffect()
        {
            Debug.LogFormat("<color=cyan>StopPortalSummonEffect not implemented</color>");
        }
    }
}
