using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilityVortexPull : BossAbility
    {
        [Header("Settings")]
        [SerializeField] private float duration = 5f;
        [SerializeField] private float range = 7f;
        [SerializeField] private float pullForce = 10f;
        
        private float _timer;
        
        public override void Activate(Boss boss)
        {
            _timer = 0f;
            boss.PlayVortexEffect();
        }

        public override void UpdateAbility(Boss boss, float deltaTime)
        {
            _timer += Time.deltaTime;   
            
            ApplyVortexPull(boss);
            
            if (_timer >= duration)
            {
                boss.SetState(new BossWanderState());
            }
        }
        
        public override void Deactivate(Boss boss)
        {
            boss.StopVortexEffect();
        }

        private void ApplyVortexPull(Boss boss)
        {
            var results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(boss.transform.position, range, results);
            for (var i = 0; i < size; i++)
            {
                if (results[i].CompareTag("Player"))
                {
                    var playerRigidbody = results[i].GetComponent<Rigidbody>();
                    
                    if (playerRigidbody != null)
                    {
                        var direction = (boss.transform.position - playerRigidbody.position).normalized;
                        playerRigidbody.AddForce(direction * pullForce);
                    }
                }
            }
        }
    }
}
