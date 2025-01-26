using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossWanderState : IBossState
    {
        private readonly float _wanderRadius = 10f;
        
        // The time the boss will wait before moving to another position
        private readonly float _waitTime = 2f;
        private float _timer;
        
        private NavMeshAgent _navMeshAgent;
        
        // Detection range of the boss
        private readonly float _detectionRange = 10f;
        private readonly float _fieldOfView = 60f;
        
        public void Enter(Boss boss)
        {
            _timer = 0f;
            _navMeshAgent = boss.GetMeshAgent();
            _navMeshAgent.isStopped = false;
            MoveToRandomPosition(boss);
        }

        public void Update(Boss boss)
        {
            if (IsPlayerInSight(boss))
            {
                boss.SetState(new BossChaseState());
                return;
            }

            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
            {
                _timer += Time.deltaTime;

                if (_timer >= _waitTime)
                {
                    _timer = 0f;
                    MoveToRandomPosition(boss);
                }
            }
        }

        public void Exit(Boss boss)
        {
            _navMeshAgent.isStopped = true;
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
        
        /// <summary>
        /// Check if the player is in sight of the boss
        /// </summary>
        /// <param name="boss"> The boss to check </param>
        /// <returns></returns>
        private bool IsPlayerInSight(Boss boss)
        {
            var player = Player.Instance;
            if (player == null) return false;
            Debug.DrawLine(boss.transform.position, player.transform.position, Color.yellow);

            var directionToPlayer = (player.transform.position - boss.transform.position).normalized;
            var distanceToPlayer = Vector3.Distance(boss.transform.position, player.transform.position);

            // Check if the player is within the detection range
            if (distanceToPlayer > _detectionRange) return false;
            Debug.Log("<color=cyan>Player within detection range</color>");
            Debug.DrawLine(boss.transform.position, player.transform.position, Color.green);

            // Check if the player is within the field of view
            var angleToPlayer = Vector3.Angle(boss.transform.forward, directionToPlayer);
            if (angleToPlayer > _fieldOfView / 2) return false;
            Debug.Log("<color=cyan>Player within field of view</color>");
            Debug.DrawLine(boss.transform.position, player.transform.position, Color.blue);

            // Check if the player is in sight
            if (Physics.Raycast(boss.transform.position, directionToPlayer, out var hit, _detectionRange))
            {
                Debug.Log("<color=cyan>Player in sight</color>");
                Debug.DrawRay(boss.transform.position, directionToPlayer * _detectionRange, Color.red);
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("<color=cyan>Player found</color>");
                    return true; // the player is in sight
                }
            }

            return false;
        }
    }
}