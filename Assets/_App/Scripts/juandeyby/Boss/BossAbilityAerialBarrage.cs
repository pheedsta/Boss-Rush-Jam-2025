using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilityAerialBarrage : BossAbility
    {
        [Header("Settings")]
        [SerializeField] private float duration = 5f;
        [SerializeField] private float range = 0.5f;
        [SerializeField] private float projectileCount = 10f;
        
        private float _timer;
        
        public override void Activate(Boss boss)
        {
            _timer = 0f;
            ApplyAerialBarrage(boss);
            boss.PlayAerialBarrageEffect();
        }
        
        public override void UpdateAbility(Boss boss, float deltaTime)
        {
            _timer += deltaTime;
            
            if (_timer >= duration)
            {
                boss.SetState(new BossWanderState());
            }
        }
        
        public override void Deactivate(Boss boss)
        {
            boss.StopAerialBarrageEffect();
        }
        
        private void ApplyAerialBarrage(Boss boss)
        {
            for (var i = 0; i < projectileCount; i++)
            {
                var direction = Random.insideUnitCircle.normalized;
                var position = boss.transform.position + new Vector3(direction.x, 3f, direction.y) * range;
                var origin = boss.transform.position;

                var projectile = ServiceLocator.Get<ProjectileManager>().GetProjectile();
                projectile.transform.position = position;
                projectile.Config(origin);
            }
        }
    }
}
