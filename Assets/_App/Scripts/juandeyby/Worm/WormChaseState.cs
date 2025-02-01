using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby
{
    public class WormChaseState : IWormState
    {
        private NavMeshAgent _navMeshAgent;
        
        private readonly float _detectRange = 1.5f;
        
        public void Enter(Worm worm)
        {
            _navMeshAgent = worm.MeshAgent;
            _navMeshAgent.isStopped = false;
            worm.WormAnimator.PlayWalk();
        }

        public void Update(Worm worm)
        {
            var distance = Vector3.Distance(worm.transform.position, Player.Instance.transform.position);
            if (distance <= _detectRange)
            {
                worm.SetState(new WormAttackState());
            }
            else
            {
                _navMeshAgent.SetDestination(Player.Instance.transform.position);
            }
        }

        public void Exit(Worm worm)
        {
            _navMeshAgent.isStopped = true;
        }
    }
}