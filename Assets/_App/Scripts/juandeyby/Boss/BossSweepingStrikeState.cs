using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossSweepingStrikeState : IBossState
    {
        private BossAbilitySweepingStrike _bossAbilitySweepingStrike;
        
        public void Enter(Boss boss)
        {
            _bossAbilitySweepingStrike = boss.GetComponent<BossAbilitySweepingStrike>();
            if (_bossAbilitySweepingStrike != null)
            {
                _bossAbilitySweepingStrike.Activate(boss);
            }
        }

        public void Update(Boss boss)
        {
            if (_bossAbilitySweepingStrike != null)
            {
                _bossAbilitySweepingStrike.UpdateAbility(boss, Time.deltaTime);
            }
        }

        public void Exit(Boss boss)
        {
            if (_bossAbilitySweepingStrike != null)
            {
                _bossAbilitySweepingStrike.Deactivate(boss);
            }
        }
    }
}