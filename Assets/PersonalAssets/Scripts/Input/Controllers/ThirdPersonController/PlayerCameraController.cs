namespace ExoLab.Input
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Handles camera rotation, neck rotation, and body tracking.
    /// Exposes CameraYaw for movement direction calculation.
    /// </summary>
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Cinemachine")]
        [SerializeField, Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        private GameObject cinemachineCameraTarget;

        [SerializeField, Tooltip("Character's neck (rotates with camera yaw, clamped)")]
        private GameObject neck;

        [SerializeField, Tooltip("Character's body (smoothly rotates toward camera yaw, clamped)")]
        private GameObject body;

        [SerializeField, Tooltip("How fast the body rotates toward the camera direction")]
        private float bodyRotationSmoothTime = 0.12f;

        [SerializeField, Tooltip("How far in degrees can you move the camera up")]
        private float topClamp = 70.0f;

        [SerializeField, Tooltip("How far in degrees can you move the camera down")]
        private float bottomClamp = -30.0f;

        [SerializeField, Tooltip("How far in degrees can you move the neck/body left")]
        private float leftClamp = -90.0f;

        [SerializeField, Tooltip("How far in degrees can you move the neck/body right")]
        private float rightClamp = 90.0f;

        [SerializeField, Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        private float cameraAngleOverride = 0.0f;

        [SerializeField, Tooltip("For locking the camera position on all axis")]
        private bool lockCameraPosition = false;

        // cinemachine
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;
        private CharacterInputs input;
        private PlayerInput playerInput;

        // body rotation smoothing
        private float bodyRotationVelocity;

        private const float threshold = 0.01f;

        /// <summary>
        /// The raw camera yaw in world space (before Cinemachine smoothing).
        /// Used by PlayerMovementController for movement direction.
        /// </summary>
        public float CameraYaw => this.cinemachineTargetYaw;

        private bool IsCurrentDeviceMouse
        {
            get
            {
                return this.playerInput.currentControlScheme == "KeyboardMouse";
            }
        }

        private void Awake()
        {
            this.input = GetComponent<CharacterInputs>();
            this.playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            if (this.cinemachineCameraTarget != null)
            {
                this.cinemachineTargetYaw = this.cinemachineCameraTarget.transform.rotation.eulerAngles.y;
            }
        }

        public void ProcessCameraRotation()
        {
            if (this.input.Look.sqrMagnitude >= threshold && !this.lockCameraPosition)
            {
                float deltaTimeMultiplier = this.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                this.cinemachineTargetYaw += this.input.Look.x * deltaTimeMultiplier;
                this.cinemachineTargetPitch += this.input.Look.y * deltaTimeMultiplier;
            }

            this.cinemachineTargetYaw = ClampAngle(this.cinemachineTargetYaw, float.MinValue, float.MaxValue);
            this.cinemachineTargetPitch = ClampAngle(this.cinemachineTargetPitch, this.bottomClamp, this.topClamp);

            if (this.cinemachineCameraTarget != null)
            {
                this.cinemachineCameraTarget.transform.rotation = Quaternion.Euler(
                    this.cinemachineTargetPitch + this.cameraAngleOverride,
                    this.cinemachineTargetYaw,
                    0.0f);
            }

            float characterYaw = this.transform.eulerAngles.y;
            float relativeYaw = Mathf.DeltaAngle(characterYaw, this.cinemachineTargetYaw);
            float clampedRelativeYaw = Mathf.Clamp(relativeYaw, this.leftClamp, this.rightClamp);
            float clampedWorldYaw = characterYaw + clampedRelativeYaw;

            if (this.neck != null)
            {
                this.neck.transform.rotation = Quaternion.Euler(0f, clampedWorldYaw, 0f);
            }

            if (this.body != null)
            {
                float smoothedYaw = Mathf.SmoothDampAngle(
                    this.body.transform.eulerAngles.y,
                    clampedWorldYaw,
                    ref this.bodyRotationVelocity,
                    this.bodyRotationSmoothTime);

                this.body.transform.rotation = Quaternion.Euler(0f, smoothedYaw, 0f);
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}