namespace ExoLab.Input
{
    using Assets.PersonalAssets.Scripts.Input.Helpers;
    using UnityEngine;
    using UnityEngine.UI;

    public class MainMenuInput : MonoBehaviour, ISubsribable
    {
        [SerializeField] private CommandManager commandManager;

        [Header("Start")]
        [SerializeField] private Button startButton;
        [SerializeField] private GameObject startMenuWindow;
        [SerializeField] private GameObject mainMenuWindow;

        [Header("Return to hub")]
        [SerializeField] private Button returnHubButton;

        [Header("Options")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private GameObject settingsWindow;

        [Header("Exit")]
        [Tooltip("Dialog box caller button")]
        [SerializeField] private Button exitButton;
        [Tooltip("Last exit button")]
        [SerializeField] private Button finalExitButton;
        [SerializeField] private GameObject exitWindow;

        public bool MainMenuIsOpen { get; private set; } = true;

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
            this.returnHubButton.onClick.AddListener(OnHubReturnClicked);

            InteractionInputController.OnEscapePressed += this.EscapeHandler;
        }

        public void UnsubscribeEvents()
        {
            this.startButton.onClick.RemoveListener(OnStartClicked);
            this.settingsButton.onClick.RemoveListener(OnSettingsClicked);
            this.exitButton.onClick.RemoveListener(OnExitClicked);
            this.finalExitButton.onClick.RemoveListener(OnFinalExitClicked);
            this.returnHubButton.onClick.RemoveListener(OnHubReturnClicked);

            InteractionInputController.OnEscapePressed -= this.EscapeHandler;
        }

        private void EscapeHandler()
        {
            this.ToggleMainMenu();
        }

        private void ToggleMainMenu()
        {
            if (this.MainMenuIsOpen || this.commandManager.CanUndo)
            {
                return;
            }

            var state = !this.mainMenuWindow.activeInHierarchy;
            this.mainMenuWindow.SetActive(state);

            CursorStateController.Instance.ToggleCursor(state);
            CharacterInputs.Instance.ToggleCursorInputForLook(!state);
            CharacterInputs.Instance.SetMove(Vector2.zero);
            CharacterInputs.Instance.SetLook(Vector2.zero);
        }

        /// <summary>
        /// Launches the game. Enable player
        /// </summary>
        private void OnStartClicked()
        {
            this.EnableHub(false);
        }

        private void OnHubReturnClicked()
        {
            this.EnableHub(true);
        }

        private void EnableHub(bool isActive)
        {
            this.MainMenuIsOpen = isActive;

            this.startMenuWindow.SetActive(isActive);
            this.mainMenuWindow.SetActive(isActive);

            this.startButton.gameObject.SetActive(isActive);
            this.returnHubButton.gameObject.SetActive(!isActive);

            InputControllersManager.Instance.PlayerArmature.SetActive(!isActive);
            CharacterInputs.Instance.ToggleCursorInputForLook(!isActive);
            CursorStateController.Instance.ToggleCursor(isActive);
        }

        private void OnSettingsClicked() 
        {
            var isActive = !this.settingsWindow.activeInHierarchy;
            var command = new ChangeWindowStateCommand(this.settingsWindow, isActive);
            this.commandManager.ExecuteCommand(command);
        }

        private void OnExitClicked()
        {
            var isActive = !this.exitWindow.activeInHierarchy;
            var command = new ChangeWindowStateCommand(this.exitWindow, isActive);
            this.commandManager.ExecuteCommand(command);
        }

        private void OnFinalExitClicked()
        {
            Application.Quit();
        }
    }
}
