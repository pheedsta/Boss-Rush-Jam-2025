using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _App.Scripts.juandeyby
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator Animator => animator;
        private int _horizontal;
        private int _vertical;

        private void Awake()
        {
            _horizontal = Animator.StringToHash("Horizontal");
            _vertical = Animator.StringToHash("Vertical");
        }
        
        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            // animator.applyRootMotion = isInteracting;
            animator.SetBool("IsInteracting", isInteracting);
            animator.CrossFade(targetAnimation, 0.2f);
        }

        public void UpdateAnimatorValues(float horizontalValue, float verticalValue)
        {
            animator.SetFloat(_horizontal, horizontalValue);
            animator.SetFloat(_vertical, verticalValue);
        }
    }
}
