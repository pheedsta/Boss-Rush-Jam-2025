using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossAbilityAerialBarrage : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float duration = 5f;
        [SerializeField] private float range = 10f;
        
        private float _timer;
        
        public void Activate(Boss boss)
        {
            _timer = 0f;
            boss.PlayAerialBarrageEffect();
        }
        
        public void UpdateAbility(Boss boss, float deltaTime)
        {
            _timer += deltaTime;
            
            ApplyAerialBarrage(boss);
            
            if (_timer >= duration)
            {
                boss.SetState(new BossWanderState());
            }
        }
        
        public void Deactivate(Boss boss)
        {
            boss.StopAerialBarrageEffect();
        }
        
        private void ApplyAerialBarrage(Boss boss)
        {
            
        }
    }
}
