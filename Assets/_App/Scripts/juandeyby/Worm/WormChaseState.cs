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
            var targetPosition = Player.Instance.transform.position;
            var path = new NavMeshPath(); 
            if (distance <= _detectRange)
            {
                worm.SetState(new WormAttackState());
            } else if (_navMeshAgent.CalculatePath(targetPosition, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                _navMeshAgent.SetDestination(targetPosition);
            }
        }

        public void Exit(Worm worm)
        {
            
        }
    }
}