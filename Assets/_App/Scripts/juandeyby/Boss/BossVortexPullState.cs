using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossVortexPullState : IBossState
    {
        private BossAbilityVortexPull _bossAbilityVortexPull;
        
        public void Enter(Boss boss)
        {
            _bossAbilityVortexPull = boss.GetComponent<BossAbilityVortexPull>();
            if (_bossAbilityVortexPull != null)
            {
                _bossAbilityVortexPull.Activate(boss);
            }
        }

        public void Update(Boss boss)
        {
            if (_bossAbilityVortexPull != null)
            {
                _bossAbilityVortexPull.UpdateAbility(boss, Time.deltaTime);
            }
        }

        public void Exit(Boss boss)
        {
            if (_bossAbilityVortexPull != null)
            {
                _bossAbilityVortexPull.Deactivate(boss);
            }
        }
    }
}