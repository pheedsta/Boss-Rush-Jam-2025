using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilitySweepingStrike : BossAbility
    {
        [Header("Settings")]
        [SerializeField] private float duration = 3f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float radius = 5f;
    
        private float _timer;
        private Player _player;

        public override void Activate(Boss boss)
        {
            _timer = 0f;
            _player = Player.Instance;
            
            // ApplySweepingStrike(boss);
            boss.PlaySweepingStrikeEffect();
            
            // Play animation
            boss.BossAnimator.PlaySweepingStrike();
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
            boss.StopSweepingStrikeEffect();
        }
    }
}
