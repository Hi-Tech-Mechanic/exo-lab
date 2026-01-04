namespace ExoLab.Data
{
    using UnityEngine;
    using ExoLab.Constants;

    public class InterfaceCache
    {
        private Canvas _mainCanvas;
        private Canvas _hudCanvas;

        /// <summary>
        /// Главный холст на сцене
        /// </summary>
        public Canvas MainCanvas
        {
            get
            {
                if (this._mainCanvas == null)
                {
                    var gameObject = GameObject.FindWithTag(Constants.Tags.MainCanvas);
                    if (gameObject == null)
                       {
                        Debug.LogError($"Не найден объект с тегом {Constants.Tags.MainCanvas}");
                        return null;
                    }

                    this._mainCanvas = gameObject.GetComponent<Canvas>();
                    if (this._mainCanvas == null)
                    {
                        Debug.LogError($"Объект с тегом {Constants.Tags.MainCanvas} не содержит {nameof(Canvas)}");
                        return null;
                    }
                }

                return this._mainCanvas;
            }
        }

        public Canvas HudCanvas
        {
            get
            {
                if (this._hudCanvas == null)
                {
                    var gameObject = GameObject.FindWithTag(Constants.Tags.HudCanvas);
                    if (gameObject == null)
                    {
                        Debug.LogError($"Не найден объект с тегом {Constants.Tags.HudCanvas}");
                        return null;
                    }

                    this._hudCanvas = gameObject.GetComponent<Canvas>();
                    if (this._hudCanvas == null)
                    {
                        Debug.LogError($"Объект с тегом {Constants.Tags.HudCanvas} не содержит {nameof(Canvas)}");
                        return null;
                    }
                }

                return this._hudCanvas;
            }
        }
    }
}
