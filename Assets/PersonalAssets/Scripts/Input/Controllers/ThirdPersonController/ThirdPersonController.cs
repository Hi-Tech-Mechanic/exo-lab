/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace ExoLab.Input
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerCameraController))]
    [RequireComponent(typeof(PlayerAudioController))]
    [RequireComponent(typeof(PlayerAnimationController))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Dependencies (auto-assigned via GetComponent)")]
        [SerializeField]
        private PlayerMovementController movementController;

        [SerializeField]
        private PlayerCameraController cameraController;

        [SerializeField]
        private PlayerAnimationController animationController;

        [SerializeField]
        private PlayerAudioController audioController;

        private bool hasAnimator;

        private void Awake()
        {
            if (this.movementController == null)
                this.movementController = GetComponent<PlayerMovementController>();

            if (this.cameraController == null)
                this.cameraController = GetComponent<PlayerCameraController>();

            if (this.animationController == null)
                this.animationController = GetComponent<PlayerAnimationController>();

            if (this.audioController == null)
                this.audioController = GetComponent<PlayerAudioController>();
        }

        private void Start()
        {
            this.hasAnimator = TryGetComponent(out Animator _);
        }

        private void Update()
        {
            this.hasAnimator = TryGetComponent(out Animator _);

            if (this.movementController != null)
                this.movementController.ProcessMovement();

            if (this.hasAnimator && this.animationController != null && this.movementController != null)
            {
                this.animationController.UpdateGrounded(this.movementController.IsGrounded);

                if (this.movementController.IsJumping)
                {
                    this.animationController.UpdateJump(true);
                    this.animationController.UpdateFreeFall(false);
                }
                else if (this.movementController.IsFalling)
                {
                    this.animationController.UpdateJump(false);
                    this.animationController.UpdateFreeFall(true);
                }
                else
                {
                    this.animationController.UpdateJump(false);
                    this.animationController.UpdateFreeFall(false);
                }

                this.animationController.UpdateMovement(
                    this.movementController.AnimationBlend,
                    this.movementController.InputMagnitude);
            }
        }

        private void LateUpdate()
        {
            if (this.cameraController != null)
                this.cameraController.ProcessCameraRotation();
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (this.audioController != null)
                this.audioController.OnFootstep(animationEvent);
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (this.audioController != null)
                this.audioController.OnLand(animationEvent);
        }
    }
}