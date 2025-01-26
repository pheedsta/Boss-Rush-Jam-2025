using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int Jumping = Animator.StringToHash("Jump");
        [SerializeField] private Animator animator;
        
        public void Jump()
        {
            animator.SetTrigger(Jumping);
        }
    }
}
