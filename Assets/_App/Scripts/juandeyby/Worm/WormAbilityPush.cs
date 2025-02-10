using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class WormAbilityPush : WormAbility
    {
        [Header("Settings")]
        [SerializeField] private float force = 4f;
        [SerializeField] private float duration = 2f;
        [SerializeField] private int damage = 4;
        
        private float _timer;
        
        public override void Activate(Worm worm)
        {
            _timer = 0f;
            worm.MeshAgent.isStopped = true;
            
            ApplyForcePush(worm);
            // worm.PlayForsePushEffect();
        }

        public override void UpdateAbility(Worm worm, float deltaTime)
        {
            _timer += deltaTime;
            
            if (_timer >= duration)
            {
                worm.SetState(new WormChaseState());
            }
        }

        public override void Deactivate(Worm worm)
        {
            // worm.StopForsePushEffect();
        }
        
        /// <summary>
        /// Apply force push to player
        /// </summary>
        /// <param name="worm"> Worm </param>
        private void ApplyForcePush(Worm worm)
        {
            var player = Player.Instance;
            var direction = (player.transform.position - worm.transform.position).normalized;
            direction = direction.normalized;

            var playerLocomotion = player.GetComponent<PlayerLocomotion>();
            playerLocomotion.Stroke(direction, force);
            
            var playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
    }
}