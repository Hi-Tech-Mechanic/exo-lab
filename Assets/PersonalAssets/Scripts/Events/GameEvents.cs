namespace ExoLab
{
    using ExoLab.Data;
    using System;
    using UnityEngine;

    public static partial class GameEvents
    {
        /// <summary>
        /// События, связанные с созданием скриншотов объектов сцены.
        /// </summary>
        public static class ScreenshotEvents
        {
            /// <summary>
            /// Событие захвата скриншота объекта.
            /// Передаёт объект для съёмки и имя файла.
            /// </summary>
            public static event Action<GameObject, string> OnScreenshotRequested;

            /// <summary>
            /// Вызвать событие захвата скриншота объекта.
            /// </summary>
            /// <param name="target">Объект, который нужно сфотографировать</param>
            /// <param name="fileName">Имя файла без расширения</param>
            public static void RaiseScreenshotRequested(GameObject target, string fileName)
            {
                OnScreenshotRequested?.Invoke(target, fileName);
            }
        }
    }
}
