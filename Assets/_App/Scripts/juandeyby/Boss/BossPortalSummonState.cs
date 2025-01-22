using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossPortalSummonState : IBossState
    {
        private BossAbilityPortalSummon _bossAbilityPortalSummon;
    
        public void Enter(Boss boss)
        {
            _bossAbilityPortalSummon = boss.GetComponent<BossAbilityPortalSummon>();
            if (_bossAbilityPortalSummon != null)
            {
                _bossAbilityPortalSummon.Activate(boss);
            }
        }

        public void Update(Boss boss)
        {
            if (_bossAbilityPortalSummon != null)
            {
                _bossAbilityPortalSummon.UpdateAbility(boss, Time.deltaTime);
            }
        }

        public void Exit(Boss boss)
        {
            if (_bossAbilityPortalSummon != null)
            {
                _bossAbilityPortalSummon.Deactivate(boss);
            }
        }
    }
}
