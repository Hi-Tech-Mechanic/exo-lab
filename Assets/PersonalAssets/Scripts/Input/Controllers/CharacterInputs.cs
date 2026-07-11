namespace ExoLab.Input
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CharacterInputs : MonoBehaviour
	{
		public static CharacterInputs Instance;
        
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        private void Awake()
        {
			Instance = this;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
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

		private void SetCursorState(bool isLocked)
		{
			Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}
