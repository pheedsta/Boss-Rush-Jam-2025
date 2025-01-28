using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilityAttack : BossAbility
    {
        private static readonly int Attack = Animator.StringToHash("Attack");

        [Header("Settings")]
        [SerializeField] private Animator _animator;
        
        public override void Activate(Boss boss)
        {
            _animator.SetTrigger(Attack);
        }

        public override void UpdateAbility(Boss boss, float deltaTime)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                boss.SetState(new BossChaseState());
            }
        }

        public override void Deactivate(Boss boss)
        {
            _animator.ResetTrigger("Attack");
        }
    }
}
