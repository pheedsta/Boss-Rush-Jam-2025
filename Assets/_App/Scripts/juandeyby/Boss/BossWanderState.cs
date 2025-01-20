using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossWanderState : IBossState
    {
        private readonly float _wanderRadius = 10f;
        
        // The time the boss will wait before moving to another position
        private readonly float _waitTime = 2f;
        private float _timer = 0f;
        
        // The number of times the boss will wander
        private int _wanderCount = 0;
        private readonly int _maxWanderCount = 3;
        
        private NavMeshAgent _navMeshAgent;
        
        public void Enter(Boss boss)
        {
            _navMeshAgent = boss.GetMeshAgent();
            MoveToRandomPosition(boss);
        }

        public void Update(Boss boss)
        {
            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
            {
                _timer += Time.deltaTime;
                
                if (_timer >= _waitTime)
                {
                    _wanderCount++;
                    if (_wanderCount >= _maxWanderCount)
                    {
                        Exit(boss);
                        // boss.SetState(new BossVortexPullState());
                        boss.SetState(new BossSweepingStrikeState());
                    }
                    else
                    {
                        MoveToRandomPosition(boss);
                    }
                    _timer = 0f;
                }
            }
        }

        public void Exit(Boss boss)
        {
            
        }
        
        /// <summary>
        /// Move the boss to a random position
        /// </summary>
        /// <param name="boss"> The boss to move </param>
        private void MoveToRandomPosition(Boss boss)
        {
           var randomDirection = Random.insideUnitSphere * _wanderRadius;
           randomDirection += boss.transform.position;
           
           if (NavMesh.SamplePosition(randomDirection, out var hit, _wanderRadius, NavMesh.AllAreas))
           {
               _navMeshAgent.SetDestination(hit.position);
           } 
        }
    }
}