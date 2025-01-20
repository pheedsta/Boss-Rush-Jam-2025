using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilitySweepingStrike : BossAbility
    {
        [Header("Settings")]
        [SerializeField] private float duration = 3f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float radius = 5f;
        [SerializeField] private float force = 10f;
        [SerializeField] private float delay = 0.5f;
    
        private float _timer;
        private float _nextSpawnTime;
        private Vector3 _direction;

        public override void Activate(Boss boss)
        {
            _timer = 0f;
            
            var randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            _direction = boss.transform.position + randomDirection * radius;
            
            boss.PlaySweepingStrikeEffect();
        }

        public override void UpdateAbility(Boss boss, float deltaTime)
        {
            _timer += deltaTime;
                        
            if (_timer >= _nextSpawnTime)
            {
                ApplySweepingStrike(boss);
                _nextSpawnTime += delay;
            }
            
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
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = _direction;
            sphere.transform.localScale = Vector3.one * radius * 0.2f;
            Destroy(sphere, 0.2f);
            
            var results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(_direction, radius, results);
            
            for (var i = 0; i < size; i++)
            {
                if (results[i].CompareTag("Player"))
                {
                    // if (result.TryGetComponent(out IDamageable damageable))
                    // {
                    //     damageable.TakeDamage(damage);
                    // }
                }
            }
        }
    }
}
