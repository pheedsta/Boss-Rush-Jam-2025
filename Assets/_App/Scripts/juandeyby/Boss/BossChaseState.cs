using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossChaseState : IBossState
    {
        private NavMeshAgent _navMeshAgent;
        
        private readonly float _spellRange = 10f;
        private readonly float _attackRange = 3f;
        
        public void Enter(Boss boss)
        {
            _navMeshAgent = boss.MeshAgent;
            _navMeshAgent.isStopped = false;
        }

        public void Update(Boss boss)
        {
            var playerPosition = Player.Instance.transform.position;
            var playerParent = Player.Instance.transform.parent;
            var distance = Vector3.Distance(boss.transform.position, playerPosition);
    
            if (distance <= _attackRange)
            {
                Debug.Log("<color=red>Attack!</color>");
                boss.SetState(new BossAttackState());
            }
            else if (distance <= _spellRange)
            {
                Debug.Log("<color=red>Spell!</color>");
                boss.SetState(new BossSweepingStrikeState());
            }
            else if (playerParent != null && playerParent.CompareTag("B"))
            {
                _navMeshAgent.SetDestination(playerPosition);
            }
            else if (IsPathAvailable(playerPosition))
            {
                Debug.Log("<color=red>Chase!</color>");
                _navMeshAgent.SetDestination(playerPosition);
            }
            else
            {
                Debug.Log("<color=red>Wander!</color>");
                boss.SetState(new BossWanderState());
            }
        }
        
        private bool IsPathAvailable(Vector3 targetPosition)
        {
            var path = new NavMeshPath();
            return _navMeshAgent.CalculatePath(targetPosition, path) && path.status == NavMeshPathStatus.PathComplete;
        }

        public void Exit(Boss boss)
        {
            _navMeshAgent.isStopped = true;
        }
    }
}
