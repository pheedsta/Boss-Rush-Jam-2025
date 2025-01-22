using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilityPortalSummon : BossAbility
    {
        [Header("Settings")]
        [SerializeField] private float duration = 5f;
        [SerializeField] private float range = 3f;
        [SerializeField] private int portalCount = 5;
        
        private float _timer;
        
        public override void Activate(Boss boss)
        {
            _timer = 0f;
            ApplyPortalSummon(boss);
            boss.PlayPortalSummonEffect();
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
            boss.StopPortalSummonEffect();
        }
        
        private void ApplyPortalSummon(Boss boss)
        {
            for (var i = 0; i < portalCount; i++)
            {
                var randomPosition = boss.transform.position + Random.insideUnitSphere * range;
                var portal = ServiceLocator.Get<PortalManager>().GetPortal();
                portal.transform.position = randomPosition;
            }
        }
    }
}
