using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public abstract class BossAbility : MonoBehaviour
    {
        public abstract void Activate(Boss boss);
        public abstract void UpdateAbility(Boss boss, float deltaTime);
        public abstract void Deactivate(Boss boss);
    }
}
