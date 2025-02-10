using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby
{
    public class WormSpawnState : IWormState
    {
        private NavMeshAgent _navMeshAgent;
        
        public void Enter(Worm worm)
        {
            _navMeshAgent = worm.MeshAgent;
            _navMeshAgent.enabled = false;
        }

        public void Update(Worm worm)
        {
            // Check Raycast if worm is on ground
            Debug.DrawRay(worm.transform.position, Vector3.down * 0.3f, Color.red);
            if (Physics.Raycast(worm.transform.position, Vector3.down, out var hit, 0.3f))
            {
                worm.WormAnimator.PlayStandUp();
                worm.SetState(new WormChaseState());
            }
            else
            {
                worm.transform.position += Vector3.down * (Time.deltaTime * 3f);
            }
        }

        public void Exit(Worm worm)
        {
            _navMeshAgent.enabled = true;
            // worm.Rb.isKinematic = true;
        }
    }
}