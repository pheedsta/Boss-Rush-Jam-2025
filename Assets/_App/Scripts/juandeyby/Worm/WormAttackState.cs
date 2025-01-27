using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class WormAttackState : IWormState
    {
        private WormAbilityPoisonSpit _poisonSpit;
        private WormAbilityPush _push;
        
        public void Enter(Worm worm)
        {
            if (worm is IPoisonWorm)
            {
                _poisonSpit = worm.GetComponent<WormAbilityPoisonSpit>();
                _poisonSpit.Activate(worm);
            }
            else if (worm is IPushWorm)
            {
                _push = worm.GetComponent<WormAbilityPush>();
                _push.Activate(worm);
            }
        }

        public void Update(Worm worm)
        {
            if (worm is IPoisonWorm)
            {
                _poisonSpit.UpdateAbility(worm, Time.deltaTime);
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
                _poisonSpit.Deactivate(worm);
            }
            else if (worm is IPushWorm)
            {
                _push.Deactivate(worm);
            }
        }
    }
}