using UnityEngine.AI;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossDeathState : IBossState
    {
        private NavMeshAgent _navMeshAgent;
    
        public void Enter(Boss boss)
        {
            _navMeshAgent = boss.MeshAgent;
            _navMeshAgent.isStopped = true;
            
            boss.BossAnimator.PlayDead();
        }

        public void Update(Boss boss)
        {
        }

        public void Exit(Boss boss)
        {
        }
    }
}
