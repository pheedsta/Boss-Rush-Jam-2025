using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class WormAbilityPoison : WormAbility
    {
        [Header("Settings")]
        [SerializeField] private PoisonCollider poisonColliderPrefab;
        [SerializeField] private Transform poisonColliderSpawnPoint;
        [SerializeField] private ParticleSystem vfx;
        [SerializeField] private float force = 4f;
        [SerializeField] private float duration = 2f;
        
        private float _timer;
        
        public override void Activate(Worm worm)
        {
            _timer = 0f;
            worm.MeshAgent.isStopped = true;
            
            vfx.Play();
            
            var poisonCollider = Instantiate(poisonColliderPrefab, poisonColliderSpawnPoint.position, Quaternion.identity, worm.transform);
            var rbPoisonCollider = poisonCollider.GetComponent<Rigidbody>();
            rbPoisonCollider.AddForce(worm.transform.forward * force, ForceMode.Impulse);
            
            ServiceLocator.Get<MusicManager>().PlayAcidSpray();
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