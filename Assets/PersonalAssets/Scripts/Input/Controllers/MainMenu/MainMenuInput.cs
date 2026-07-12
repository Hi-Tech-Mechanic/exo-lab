namespace ExoLab.Input
{
    using Assets.PersonalAssets.Scripts.Input.Helpers;
    using UnityEngine;
    using UnityEngine.UI;

    public class MainMenuInput : MonoBehaviour, ISubsribable
    {
        [Header("Start")]
        [SerializeField] private Button startButton;
        [SerializeField] private GameObject startMenuWindow;
        [SerializeField] private GameObject mainMenuWindow;

        [Header("Options")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private GameObject settingsWindow;

        [Header("Exit")]
        [Tooltip("Dialog box caller button")]
        [SerializeField] private Button exitButton;
        [Tooltip("Last exit button")]
        [SerializeField] private Button finalExitButton;
        [SerializeField] private GameObject exitWindow;

        private void OnEnable()
        {
            this.SubscribeEvents();
        }

        private void OnDisable()
        {
            this.UnsubscribeEvents();
        }

        public void SubscribeEvents()
        {
            this.startButton.onClick.AddListener(OnStartClicked);
            this.settingsButton.onClick.AddListener(OnSettingsClicked);
            this.exitButton.onClick.AddListener(OnExitClicked);
            this.finalExitButton.onClick.AddListener(OnFinalExitClicked);

            InteractionInputController.OnEscapePressed += this.EscapeHandler;
        }

        public void UnsubscribeEvents()
        {
            this.startButton.onClick.RemoveListener(OnStartClicked);
            this.settingsButton.onClick.RemoveListener(OnSettingsClicked);
            this.exitButton.onClick.RemoveListener(OnExitClicked);
            this.finalExitButton.onClick.RemoveListener(OnFinalExitClicked);

            InteractionInputController.OnEscapePressed -= this.EscapeHandler;
        }

        private void EscapeHandler()
        {
            this.ToggleMainMenu();

            CursorStateController.Instance.ToggleCursor();
            CharacterInputs.Instance.ToggleCursorInputForLook();
            CharacterInputs.Instance.SetMove(Vector2.zero);
            CharacterInputs.Instance.SetLook(Vector2.zero);
        }

        private void ToggleMainMenu()
        {
            var state = !this.mainMenuWindow.activeInHierarchy;
            this.mainMenuWindow.SetActive(state);
        }

        /// <summary>
        /// Launches the game. Enable player
        /// </summary>
        private void OnStartClicked() 
        {
            this.startMenuWindow.SetActive(false);
            this.mainMenuWindow.SetActive(false);

            InputControllersManager.Instance.PlayerArmature.SetActive(true);
            CharacterInputs.Instance.ToggleCursorInputForLook(true);
            CursorStateController.Instance.ToggleCursor(false);
        }

        private void OnSettingsClicked() 
        {
            var state = !this.settingsWindow.activeInHierarchy;
            this.settingsWindow.SetActive(state);
        }

        private void OnExitClicked() 
        {
            var state = !this.exitWindow.activeInHierarchy;
            this.exitWindow.SetActive(state);
        }

        private void OnFinalExitClicked()
        {
            Application.Quit();
        }
    }
}
