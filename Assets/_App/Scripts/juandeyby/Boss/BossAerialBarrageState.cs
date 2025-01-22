using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAerialBarrageState : IBossState
    {
        private BossAbilityAerialBarrage _bossAbilityAerialBarrage;
    
        public void Enter(Boss boss)
        {
            _bossAbilityAerialBarrage = boss.GetComponent<BossAbilityAerialBarrage>();
            if (_bossAbilityAerialBarrage != null)
            {
                _bossAbilityAerialBarrage.Activate(boss);
            }
        }

        public void Update(Boss boss)
        {
            if (_bossAbilityAerialBarrage != null)
            {
                _bossAbilityAerialBarrage.UpdateAbility(boss, Time.deltaTime);
            }
        }

        public void Exit(Boss boss)
        {
            if (_bossAbilityAerialBarrage != null)
            {
                _bossAbilityAerialBarrage.Deactivate(boss);
            }
        }
    }
}
