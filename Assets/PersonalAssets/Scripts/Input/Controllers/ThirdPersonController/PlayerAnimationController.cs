namespace ExoLab.Input
{
    using UnityEngine;

    /// <summary>
    /// Handles animator parameter updates based on player state.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;

        // animation IDs
        private int animIDSpeed;
        private int animIDGrounded;
        private int animIDJump;
        private int animIDFreeFall;
        private int animIDMotionSpeed;

        private void Awake()
        {
            this.animator = GetComponent<Animator>();
            this.AssignAnimationIDs();
        }

        private void AssignAnimationIDs()
        {
            this.animIDSpeed = Animator.StringToHash("Speed");
            this.animIDGrounded = Animator.StringToHash("Grounded");
            this.animIDJump = Animator.StringToHash("Jump");
            this.animIDFreeFall = Animator.StringToHash("FreeFall");
            this.animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        public void UpdateMovement(float speed, float inputMagnitude)
        {
            this.animator.SetFloat(this.animIDSpeed, speed);
            this.animator.SetFloat(this.animIDMotionSpeed, inputMagnitude);
        }

        public void UpdateGrounded(bool grounded)
        {
            this.animator.SetBool(this.animIDGrounded, grounded);
        }

        public void UpdateJump(bool isJumping)
        {
            this.animator.SetBool(this.animIDJump, isJumping);
        }

        public void UpdateFreeFall(bool isFalling)
        {
            this.animator.SetBool(this.animIDFreeFall, isFalling);
        }
    }
}