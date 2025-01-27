using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAttackState : IBossState
    {
        private BossAbilityAttack _bossAbilityAttack;
    
        public void Enter(Boss boss)
        {
            _bossAbilityAttack = boss.GetComponent<BossAbilityAttack>();
            if (_bossAbilityAttack != null)
            {
                _bossAbilityAttack.Activate(boss);
            }
        }

        public void Update(Boss boss)
        {
            if (_bossAbilityAttack != null)
            {
                _bossAbilityAttack.UpdateAbility(boss, Time.deltaTime);
            }
        }

        public void Exit(Boss boss)
        {
            if (_bossAbilityAttack != null)
            {
                _bossAbilityAttack.Deactivate(boss);
            }
        }
    }
}