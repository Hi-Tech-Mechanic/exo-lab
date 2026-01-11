using ExoLab.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PersonalAssets.Scripts.UI.Base
{
    public class FloatingWindowsController : MonoBehaviour
    {
        public static FloatingWindowsController Instance;

        [SerializeField]
        private GameObject windowPrefab;

        private List<FloatingWindow> windows = new List<FloatingWindow>();
        private Transform parentTransform;

        private void Awake()
        {
            Instance = this;
            this.parentTransform = Caches.Instance.Interface.MainCanvas.transform;
        }

        /// <summary>
        /// Добавить окно на сцену
        /// </summary>
        /// <param name="content">То что будет внутри окна</param>
        /// <param name="windowName"></param>
        public void AddWindow(GameObject content, string windowName)
        {
            if (IsExistingWindows(windowName))
                return;

            var window = Instantiate(this.windowPrefab, parentTransform);
            var floatingWindow = window.GetComponent<FloatingWindow>();
            floatingWindow.InitializeWindow(content, windowName);

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
