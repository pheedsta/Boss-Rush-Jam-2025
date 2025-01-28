using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class WormController : MonoBehaviour
    {
        [SerializeField] private Worm worm;
        
        private void Start()
        {
            worm.SetState(new WormSpawnState());
        }
    }
}
