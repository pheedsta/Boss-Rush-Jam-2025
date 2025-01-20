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

        public override void Activate(Boss boss)
        {
            _timer = 0f;
            
            boss.PlaySweepingStrikeEffect();
        }

        public override void UpdateAbility(Boss boss, float deltaTime)
        {
            _timer += deltaTime;
                        
            ApplySweepingStrike(boss);
            
            if (_timer >= duration)
            {
                boss.SetState(new BossWanderState());
            }
        }

        public override void Deactivate(Boss boss)
        {
            boss.StopSweepingStrikeEffect();
        }
        
        private void ApplySweepingStrike(Boss boss)
        {
            var startPosition = boss.transform.position;
            var endPosition = boss.transform.forward;

            var distance = radius;
            
            var results = new RaycastHit[10];
            var size = Physics.SphereCastNonAlloc(startPosition, radius, endPosition, results, distance);
            for (var i = 0; i < size; i++)
            {
                if (results[i].collider.CompareTag("Player"))
                {
                    
                }
            }
            
            Debug.DrawLine(startPosition, startPosition + endPosition * distance, Color.red, 1f);
        }
    }
}
