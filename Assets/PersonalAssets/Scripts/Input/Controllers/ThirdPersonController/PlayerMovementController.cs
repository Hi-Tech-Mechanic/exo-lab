namespace ExoLab.Input
{
    using UnityEngine;

    /// <summary>
    /// Handles player movement, jumping, and gravity.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Tooltip("Move speed of the character in m/s")]
        private float moveSpeed = 2.0f;

        [SerializeField, Tooltip("Sprint speed of the character in m/s")]
        private float sprintSpeed = 5.335f;

        [SerializeField, Range(0.0f, 0.3f), Tooltip("How fast the character turns to face movement direction")]
        private float rotationSmoothTime = 0.12f;

        [SerializeField, Tooltip("Acceleration and deceleration")]
        private float speedChangeRate = 10.0f;

        [Header("Jump")]
        [SerializeField, Tooltip("The height the player can jump")]
        private float jumpHeight = 1.2f;

        [SerializeField, Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        private float gravity = -15.0f;

        [SerializeField, Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        private float jumpTimeout = 0.50f;

        [SerializeField, Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        private float fallTimeout = 0.15f;

        [Header("Grounded")]
        [SerializeField, Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        private bool grounded = true;

        [SerializeField, Tooltip("Useful for rough ground")]
        private float groundedOffset = -0.14f;

        [SerializeField, Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        private float groundedRadius = 0.28f;

        [SerializeField, Tooltip("What layers the character uses as ground")]
        private LayerMask groundLayers;

        private CharacterController controller;
        private CharacterInputs input;
        private PlayerCameraController cameraController;

        // movement state
        private float speed;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float terminalVelocity = 53.0f;

        // timeout deltatime
        private float jumpTimeoutDelta;
        private float fallTimeoutDelta;

        private const float threshold = 0.01f;

        public float AnimationBlend => this.animationBlend;
        public float InputMagnitude { get; private set; }
        public bool IsGrounded => this.grounded;
        public bool IsJumping { get; private set; }
        public bool IsFalling { get; private set; }

        private void Awake()
        {
            this.controller = GetComponent<CharacterController>();
            this.input = GetComponent<CharacterInputs>();
            this.cameraController = GetComponent<PlayerCameraController>();
        }

        private void Start()
        {
            this.jumpTimeoutDelta = this.jumpTimeout;
            this.fallTimeoutDelta = this.fallTimeout;
        }

        public void ProcessMovement()
        {
            this.GroundedCheck();
            this.JumpAndGravity();
            this.Move();
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(
                this.transform.position.x,
                this.transform.position.y - this.groundedOffset,
                this.transform.position.z);

            this.grounded = Physics.CheckSphere(
                spherePosition,
                this.groundedRadius,
                this.groundLayers,
                QueryTriggerInteraction.Ignore);
        }

        private void Move()
        {
            float targetSpeed = this.input.Sprint ? this.sprintSpeed : this.moveSpeed;

            if (this.input.Move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            float currentHorizontalSpeed = new Vector3(
                this.controller.velocity.x,
                0.0f,
                this.controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            this.InputMagnitude = this.input.AnalogMovement
                ? this.input.Move.magnitude
                : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                this.speed = Mathf.Lerp(
                    currentHorizontalSpeed,
                    targetSpeed * this.InputMagnitude,
                    Time.deltaTime * this.speedChangeRate);

                this.speed = Mathf.Round(this.speed * 1000f) / 1000f;
            }
            else
            {
                this.speed = targetSpeed;
            }

            this.animationBlend = Mathf.Lerp(
                this.animationBlend,
                targetSpeed,
                Time.deltaTime * this.speedChangeRate);

            if (this.animationBlend < 0.01f)
            {
                this.animationBlend = 0f;
            }

            Vector3 inputDirection = new Vector3(
                this.input.Move.x,
                0.0f,
                this.input.Move.y).normalized;

            if (this.input.Move != Vector2.zero)
            {
                float cameraYaw = this.cameraController != null
                    ? this.cameraController.CameraYaw
                    : 0f;

                this.targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
                    + cameraYaw;

                float rotation = Mathf.SmoothDampAngle(
                    this.transform.eulerAngles.y,
                    this.targetRotation,
                    ref this.rotationVelocity,
                    this.rotationSmoothTime);

                this.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, this.targetRotation, 0.0f) * Vector3.forward;

            this.controller.Move(
                targetDirection.normalized * (this.speed * Time.deltaTime)
                + new Vector3(0.0f, this.verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void JumpAndGravity()
        {
            if (this.grounded)
            {
                this.fallTimeoutDelta = this.fallTimeout;

                this.IsJumping = false;
                this.IsFalling = false;

                if (this.verticalVelocity < 0.0f)
                {
                    this.verticalVelocity = -2f;
                }

                if (this.input.Jump && this.jumpTimeoutDelta <= 0.0f)
                {
                    this.verticalVelocity = Mathf.Sqrt(this.jumpHeight * -2f * this.gravity);
                    this.IsJumping = true;
                }

                if (this.jumpTimeoutDelta >= 0.0f)
                {
                    this.jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                this.jumpTimeoutDelta = this.jumpTimeout;

                if (this.fallTimeoutDelta >= 0.0f)
                {
                    this.fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    this.IsFalling = true;
                }

                this.input.SetJump(false);
            }

            if (this.verticalVelocity < this.terminalVelocity)
            {
                this.verticalVelocity += this.gravity * Time.deltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (this.grounded)
            {
                Gizmos.color = transparentGreen;
            }
            else
            {
                Gizmos.color = transparentRed;
            }

            Gizmos.DrawSphere(
                new Vector3(
                    this.transform.position.x,
                    this.transform.position.y - this.groundedOffset,
                    this.transform.position.z),
                this.groundedRadius);
        }
    }
}