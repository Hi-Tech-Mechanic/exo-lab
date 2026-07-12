using UnityEngine;

namespace Assets.PersonalAssets.Scripts.Input.Helpers
{
    internal class CursorStateController : MonoBehaviour
    {
        public static CursorStateController Instance;
        
        /// <summary>
        /// В начале игры курсор должен быть виден
        /// </summary>
        [SerializeField] private bool cursorEnabled = true;

        private void Awake()
        {
            this.Init();
        }

        public void ToggleCursor(bool? isActive = null)
        {
            if (isActive == null)
            {
                isActive = !this.cursorEnabled;
            }

            this.SetCursorState((bool)isActive);
        }

        private void Init()
        {
            Instance = this;
            DontDestroyOnLoad(this);

            // Init cursor state
            this.ToggleCursor(this.cursorEnabled);
        }

        private void SetCursorState(bool isActive)
        {
            this.cursorEnabled = isActive;

            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isActive;
        }
    }
}
