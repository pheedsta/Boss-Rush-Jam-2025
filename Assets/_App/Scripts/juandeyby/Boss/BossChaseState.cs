using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossChaseState : IBossState
    {
        private NavMeshAgent _navMeshAgent;
        
        private readonly float _detectionRange = 10f;
        private readonly float _spellRange = 6f;
        private readonly float _attackRange = 4f;
        
        public void Enter(Boss boss)
        {
            _navMeshAgent = boss.MeshAgent;
            _navMeshAgent.isStopped = false;
        }

        public void Update(Boss boss)
        {
            var distance = Vector3.Distance(boss.transform.position, Player.Instance.transform.position);
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
            else if (distance <= _detectionRange)
            {
                Debug.Log("<color=red>Chase!</color>");
                _navMeshAgent.SetDestination(Player.Instance.transform.position);
            }
            else
            {
                Debug.Log("<color=red>Wander!</color>");
                boss.SetState(new BossWanderState());
            }
        }

        public void Exit(Boss boss)
        {
            _navMeshAgent.isStopped = true;
        }
    }
}
