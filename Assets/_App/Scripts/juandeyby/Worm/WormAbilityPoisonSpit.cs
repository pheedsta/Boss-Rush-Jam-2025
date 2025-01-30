namespace _App.Scripts.juandeyby
{
    public class WormAbilityPoisonSpit : WormAbility
    {
        public override void Activate(Worm worm)
        {
            worm.SetState(new WormChaseState());
        }

        public override void UpdateAbility(Worm worm, float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public override void Deactivate(Worm worm)
        {
            throw new System.NotImplementedException();
        }
    }
}