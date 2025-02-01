using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class WormAttackState : IWormState
    {
        private WormAbilityPoison _poison;
        private WormAbilityPush _push;
        
        public void Enter(Worm worm)
        {
            if (worm is IPoisonWorm)
            {
                _poison = worm.GetComponent<WormAbilityPoison>();
                _poison.Activate(worm);
            }
            else if (worm is IPushWorm)
            {
                _push = worm.GetComponent<WormAbilityPush>();
                _push.Activate(worm);
            }
            worm.WormAnimator.PlayAttack();
        }

        public void Update(Worm worm)
        {
            if (worm is IPoisonWorm)
            {
                _poison.UpdateAbility(worm, Time.deltaTime);
            }
            else if (worm is IPushWorm)
            {
                _push.UpdateAbility(worm, Time.deltaTime);
            }
        }

        public void Exit(Worm worm)
        {
            if (worm is IPoisonWorm)
            {
                _poison.Deactivate(worm);
            }
            else if (worm is IPushWorm)
            {
                _push.Deactivate(worm);
            }
        }
    }
}