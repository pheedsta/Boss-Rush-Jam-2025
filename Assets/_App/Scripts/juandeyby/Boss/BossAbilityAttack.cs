using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilityAttack : BossAbility
    {
        [Header("Settings")]
        [SerializeField] private float duration = 1f;
        
        private float _timer;
        private Player _player;
        
        public override void Activate(Boss boss)
        {
            _timer = 0f;
            _player = Player.Instance;
            boss.BossAnimator.PlayAttack();
        }

        public override void UpdateAbility(Boss boss, float deltaTime)
        {
            _timer += deltaTime;
            
            // Rotate towards player with delta time.
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation,
                Quaternion.LookRotation(_player.transform.position - boss.transform.position), 0.1f);
            
            if (_timer >= duration)
            {
                boss.SetState(new BossWanderState());
            }
        }

        public override void Deactivate(Boss boss)
        {
            
        }
    }
}
