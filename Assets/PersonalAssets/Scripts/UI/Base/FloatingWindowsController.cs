using System.Collections.Generic;
using UnityEngine;

namespace Assets.PersonalAssets.Scripts.UI.Base
{
    public class FloatingWindowsController : MonoBehaviour
    {
        private static List<FloatingWindow> windows = new List<FloatingWindow>();

        public static void Add(GameObject window)
        {
            var target = window.gameObject.GetComponent<FloatingWindow>();
            windows.Add(target);
        }

        public static bool IsExistingWindows(string name)
        {
            foreach (var window in windows)
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
