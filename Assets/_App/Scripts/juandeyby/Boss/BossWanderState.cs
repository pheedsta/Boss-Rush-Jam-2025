using UnityEngine;
using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossWanderState : IBossState
    {
        private readonly float _wanderRadius = 10f;
        
        // The time the boss will wait before moving to another position
        private readonly float _waitTime = 0.5f;
        private float _timer;
        
        private NavMeshAgent _navMeshAgent;
        
        // Detection range of the boss
        private readonly float _maxSweepingDetectionRange = 20f;
        private readonly float _minSweepingDetectionRange = 7f;
        
        private readonly float _attackDetectionRange = 5f;
        private readonly float _fieldOfView = 60f;
        
        public void Enter(Boss boss)
        {
            _timer = 0f;
            _navMeshAgent = boss.GetMeshAgent();
            _navMeshAgent.isStopped = false;
            MoveToRandomPosition(boss);
            
            // Play the wander animation
            boss.BossAnimator.PlayWander();
        }

        public void Update(Boss boss)
        {
            // // Check if the player is in sight of the boss 
            // if (IsPlayerInSight(boss))
            // {
            //     boss.SetState(new BossChaseState());
            //     return;
            // }
            
            // Check if the player is close to the boss
            var playerPosition = Player.Instance.transform.position;
            var playerParent = Player.Instance.transform.parent;
            if (playerParent != null && playerParent.CompareTag("B"))
            {
                boss.SetState(new BossChaseState());
                return;
            }
            if (IsPathAvailable(playerPosition))
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

        private bool IsPathAvailable(Vector3 targetPosition)
        {
            var path = new NavMeshPath();
            return _navMeshAgent.CalculatePath(targetPosition, path) && path.status == NavMeshPathStatus.PathComplete;
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

            // Check if the random direction is valid
            if (NavMesh.SamplePosition(randomDirection, out var hit, _wanderRadius, NavMesh.AllAreas))
            {
                _navMeshAgent.SetDestination(hit.position);
            }
            else
            {
                // If the random direction is not valid, move to the opposite direction
                randomDirection = boss.transform.position - randomDirection;
        
                if (NavMesh.SamplePosition(randomDirection, out hit, _wanderRadius, NavMesh.AllAreas))
                {
                    _navMeshAgent.SetDestination(hit.position);
                }
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
            directionToPlayer.y = 0f;
            var distanceToPlayer = Vector3.Distance(boss.transform.position, player.transform.position);

            // Check if the player is within the detection range
            if (distanceToPlayer > _maxSweepingDetectionRange || distanceToPlayer < _minSweepingDetectionRange) return false;
            Debug.Log("<color=cyan>Player within detection range</color>");
            Debug.DrawLine(boss.transform.position, player.transform.position, Color.green);

            // Check if the player is within the field of view
            var angleToPlayer = Vector3.Angle(boss.transform.forward, directionToPlayer);
            if (angleToPlayer > _fieldOfView / 2) return false;
            Debug.Log("<color=cyan>Player within field of view</color>");
            Debug.DrawLine(boss.transform.position, player.transform.position, Color.blue);

            // Check if the player is in sight
            float[] heightOffsets = { 0f, 2.5f };
            foreach (var offset in heightOffsets)
            {
                var rayStart = boss.transform.position + Vector3.up * offset;
                var rayEnd = player.transform.position + Vector3.up * offset;
                var direction = (rayEnd - rayStart).normalized;
                
                Debug.DrawRay(rayStart, direction * _maxSweepingDetectionRange, Color.red);
                if (Physics.Raycast(rayStart, direction, out var hit, _maxSweepingDetectionRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("<color=cyan>Player found</color>");
                        return true; // Se encontró al jugador
                    }
                }
            }
            return false;
        }
    }
}