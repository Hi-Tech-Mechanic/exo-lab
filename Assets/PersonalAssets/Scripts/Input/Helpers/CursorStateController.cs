using UnityEngine;

namespace Assets.PersonalAssets.Scripts.Input.Helpers
{
    internal class CursorStateController : MonoBehaviour
    {
        public static CursorStateController Instance;
        
        /// <summary>
        /// В начале игры курсор должен быть виден
        /// </summary>
        private bool cursorEnabled = true;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);

            this.ToggleCursor(this.cursorEnabled);
        }

        public void ToggleCursor(bool? isActive = null)
        {
            if (isActive == null)
            {
                isActive = !this.cursorEnabled;
            }

            this.cursorEnabled = (bool)isActive;
            this.SetCursorState(this.cursorEnabled);
        }

        private void SetCursorState(bool isActive)
        {
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isActive;
        }
    }
}
