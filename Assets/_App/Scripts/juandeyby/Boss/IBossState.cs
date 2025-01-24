namespace _App.Scripts.juandeyby.Boss
{
    public interface IBossState
    {
        void Enter(Boss boss);
        void Update(Boss boss);
        void Exit(Boss boss);
    }
}
