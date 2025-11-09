namespace ExoLab.UI
{
    using UnityEngine;

    /// <summary>
    /// Переключатель вкладок
    /// </summary>
    public class TabToggle : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] tabs;

        public void SelectTab(int tabIndex)
        {
            foreach (var tab in tabs)
            {
                if (tab.activeInHierarchy == true)
                {
                    tab.SetActive(false);
                }
            }

            tabs[tabIndex].SetActive(true);
        }
    }
}
