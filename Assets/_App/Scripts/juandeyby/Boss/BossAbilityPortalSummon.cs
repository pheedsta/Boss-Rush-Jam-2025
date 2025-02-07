using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilityPortalSummon : BossAbility
    {
        [Header("Settings")]
        [SerializeField] private float duration = 5f;
        [SerializeField] private float range = 12f;
        [SerializeField] private int portalCount = 8;
        [SerializeField] private float height = 4f;
        
        private float _timer;
        
        public override void Activate(Boss boss)
        {
            _timer = 0f;
            ApplyPortalSummon(boss);
            // boss.PlayPortalSummonEffect();
            
            boss.BossAnimator.PlayPortalSummon();

            var player = Player.Instance;
            var playerLocomotion = player.GetComponent<PlayerLocomotion>();
            playerLocomotion.Poison();
        }
        
        public override void UpdateAbility(Boss boss, float deltaTime)
        {
            _timer += deltaTime;
            
            if (_timer >= duration)
            {
                boss.SetState(new BossChaseState());
            }
        }
        
        public override void Deactivate(Boss boss)
        {
            // boss.StopPortalSummonEffect();
        }
        
        private void ApplyPortalSummon(Boss boss)
        {
            var angleStep = 360f / portalCount;
            var radius = range;

            for (var i = 0; i < portalCount; i++)
            {
                var angle = i * angleStep * Mathf.Deg2Rad;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;
                var offset = new Vector3(x, height, z);
                var portalPosition = offset;

                var portal = ServiceLocator.Get<PortalManager>().GetPortal();
                portal.transform.position = portalPosition;
            }
        }
    }
}
