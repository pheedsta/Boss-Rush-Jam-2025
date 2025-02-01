using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class BossAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        public void PlayAttack()
        {
            var attack = Random.Range(0, 3);
            switch (attack)
            {
                case 0:
                    animator.CrossFade("AttackRight", 0.1f);
                    break;
                case 1:
                    animator.CrossFade("AttackLeft", 0.1f);
                    break;
                case 2:
                    animator.CrossFade("AttackBothHands", 0.1f);
                    break;
            }
        }
        
        public void SetWalk()
        {
        }

        public void PlayWander()
        {
            animator.CrossFade("Walk", 0.1f);
        }

        public void PlaySweepingStrike()
        {
            animator.CrossFade("SweepingStrike", 0.1f);
        }

        public void PlayDead()
        {
            animator.CrossFade("Dead", 0.1f);
        }
    }
}
