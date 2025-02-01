using UnityEngine;

public class WormAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void PlayStandUp()
    {
        animator.CrossFade("StandUp", 0.1f);
    }
    
    public void PlayWalk()
    {
        animator.CrossFade("Walk", 0.1f);
    }
    
    public void PlayAttack()
    {
        animator.CrossFade("Attack", 0.1f);
    }
}
