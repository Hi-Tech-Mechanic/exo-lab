namespace ExoLab.Input
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CharacterInputs : MonoBehaviour
	{
		public static CharacterInputs Instance;
        
		[Header("Character Input Values")]
        [SerializeField] private Vector2 move;
        [SerializeField] private Vector2 look;
		[SerializeField] private bool jump;
        [SerializeField] private bool sprint;

		[Header("Movement Settings")]
        [SerializeField] private bool analogMovement;

		[Header("Mouse Cursor Settings")]
        [SerializeField] private bool cursorLocked = true;
		[SerializeField] private bool cursorInputForLook = true;

        public Vector2 Move => this.move;
        public Vector2 Look => this.look;
        public bool Jump => this.jump;
        public bool Sprint => this.sprint;

        public bool AnalogMovement => this.analogMovement;

		private void Awake()
        {
			Instance = this;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            this.SetCursorState(cursorLocked);
        }

        #region Setters

        public void SetMove(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void SetLook(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void SetJump(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SetSprint(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void SetCursorState(bool isLocked)
        {
            Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        #endregion

        public void OnMove(InputValue value)
		{
			SetMove(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (this.cursorInputForLook)
			{
				SetLook(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			SetJump(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SetSprint(value.isPressed);
		}

        public void ToggleCursorInputForLook(bool? state = null)
        {
			if (state != null)
			{
				this.cursorInputForLook = (bool)state;
				return;
            }

            this.cursorInputForLook = !this.cursorInputForLook;
        }

        public void ToggleCursorLocked(bool? state = null)
        {
            if (state != null)
            {
                this.cursorLocked = (bool)state;
            }
			else
			{
                this.cursorLocked = !this.cursorLocked;
            }

            this.SetCursorState(this.cursorLocked);
        }
	}
}
