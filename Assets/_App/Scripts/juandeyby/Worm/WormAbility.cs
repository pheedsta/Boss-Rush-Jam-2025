using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public abstract class WormAbility : MonoBehaviour
    {
        public abstract void Activate(Worm worm);

        public abstract void UpdateAbility(Worm worm, float deltaTime);

        public abstract void Deactivate(Worm worm);
    }
}