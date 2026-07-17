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

        /// <summary>
        /// For locking the camera position on all axis
        /// </summary>
        public bool LockCameraPosition { get; private set; } = false;

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

            this.SetCursorInteractable((bool)isActive);
        }

        private void Init()
        {
            Instance = this;
            DontDestroyOnLoad(this);

            // Init cursor state
            this.ToggleCursor(this.cursorEnabled);
        }

        private void SetCursorInteractable(bool isActive)
        {
            this.cursorEnabled = isActive;
            this.LockCameraPosition = isActive;

            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isActive;
        }
    }
}
