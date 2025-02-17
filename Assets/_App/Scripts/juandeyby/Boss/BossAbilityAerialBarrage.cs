using System.Collections;
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
        private Coroutine _aerialBarrageCoroutine;
        
        public override void Activate(Boss boss)
        {
            _timer = 0f;
            StartCoroutine(GenerateSpiralProjectiles(boss));
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
        
        private IEnumerator GenerateSpiralProjectiles(Boss boss)
        {
            var angleStep = 40f; 
            var currentAngle = Random.Range(0f, 360f);
            var currentRange = 0.4f; 

            for (var i = 0; i < projectileCount; i++)
            {
                var radians = currentAngle * Mathf.Deg2Rad;
                var x = Mathf.Cos(radians);
                var y = Mathf.Sin(radians);

                var direction = new Vector2(x, y).normalized;
                var position = boss.transform.position +
                               new Vector3(direction.x * currentRange, 3f, direction.y * currentRange);
                var origin = boss.transform.position;

                var projectile = ServiceLocator.Get<ProjectileManager>().GetProjectile();
                projectile.transform.position = position;
                projectile.Config(origin);

                currentAngle += angleStep;
                currentRange += 0.05f;

                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
