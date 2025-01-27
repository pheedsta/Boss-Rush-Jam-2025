namespace _App.Scripts.juandeyby
{
    public interface IWormState
    {
        void Enter(Worm worm);
        void Update(Worm worm);
        void Exit(Worm worm);
    }
}