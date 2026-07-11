namespace ExoLab.Input
{
    using UnityEngine;
    using UnityEngine.UI;

    public class MainMenu : MonoBehaviour
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

        private void Awake()
        {
            this.startButton.onClick.AddListener(OnStartClicked);
            this.settingsButton.onClick.AddListener(OnSettingsClicked);
            this.exitButton.onClick.AddListener(OnExitClicked);
            this.finalExitButton.onClick.AddListener(OnFinalExitClicked);
        }

        private void OnDestroy()
        {
            this.startButton.onClick.RemoveListener(OnStartClicked);
            this.settingsButton.onClick.RemoveListener(OnSettingsClicked);
            this.exitButton.onClick.RemoveListener(OnExitClicked);
            this.finalExitButton.onClick.RemoveListener(OnFinalExitClicked);
        }

        /// <summary>
        /// Launches the game. Enable player
        /// </summary>
        private void OnStartClicked() 
        {
            this.startMenuWindow.SetActive(false);
            this.mainMenuWindow.SetActive(false);

            InputControllersManager.Instance.PlayerArmature.SetActive(true);
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
