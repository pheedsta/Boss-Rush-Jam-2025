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
            if (Physics.Raycast(worm.transform.position, Vector3.down, out var hit, 1.5f))
            {
                Debug.DrawRay(worm.transform.position, Vector3.down * 1.5f, Color.red);
                worm.SetState(new WormChaseState());
            }
        }

        public void Exit(Worm worm)
        {
            _navMeshAgent.enabled = true;
        }
    }
}