namespace ExoLab.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    public class FloatingWindowsController : MonoBehaviour
    {
        public static FloatingWindowsController Instance;

        [SerializeField]
        private GameObject windowPrefab;

        [SerializeField]
        private ItemInfoSummoner itemInfoSummoner;

        private List<FloatingWindow> windows = new List<FloatingWindow>();

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Добавить окно на сцену
        /// </summary>
        /// <param name="panel">То что будет внутри окна</param>
        /// <param name="windowName"></param>
        public void AddWindow(GameObject panel, string windowName)
        {
            if (IsExistingWindows(windowName))
            {
                return;
            }

            var window = Instantiate(this.windowPrefab, this.itemInfoSummoner.PanelsHolder);
            var floatingWindow = window.GetComponent<FloatingWindow>();
            floatingWindow.InitializeWindow(panel, windowName);

            this.windows.Add(floatingWindow);
        }

        public void DeleteWindow(GameObject window)
        {
            var target = window.gameObject.GetComponent<FloatingWindow>();
            Destroy(target.gameObject);
            this.windows.Remove(target);
        }

        private bool IsExistingWindows(string name)
        {
            foreach (var window in this.windows)
            {
                if (window.WindowName.Contains(name))
                {
                    window.SetLastPositionInHierarchy();
                    return true;
                }
            }

            return false;
        }
    }
}
