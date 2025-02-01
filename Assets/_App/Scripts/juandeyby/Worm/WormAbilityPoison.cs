using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class WormAbilityPoison : WormAbility
    {
        [Header("Settings")]
        [SerializeField] private float force = 4f;
        [SerializeField] private float duration = 2f;
        
        private float _timer;
        
        public override void Activate(Worm worm)
        {
            _timer = 0f;
            worm.MeshAgent.isStopped = true;
            
            // ApplyForcePush(worm);
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
            worm.MeshAgent.isStopped = false;
            
            // worm.StopForsePushEffect();
        }
    }
}