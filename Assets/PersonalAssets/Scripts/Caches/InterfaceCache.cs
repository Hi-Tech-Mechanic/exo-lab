namespace ExoLab.Data
{
    using UnityEngine;
    using ExoLab.Constants;

    public class InterfaceCache
    {
        private Canvas mainCanvas;
        private Canvas hudCanvas;

        /// <summary>
        /// Главный холст на сцене
        /// </summary>
        public Canvas MainCanvas
        {
            get
            {
                if (this.mainCanvas == null)
                {
                    var tag = Constants.Tags.MainCanvas;
                    var gameObject = GameObject.FindWithTag(tag);
                    if (gameObject == null)
                       {
                        Debug.LogError($"Не найден объект с тегом {tag}");
                        return null;
                    }

                    this.mainCanvas = gameObject.GetComponent<Canvas>();
                    if (this.mainCanvas == null)
                    {
                        Debug.LogError($"Объект с тегом {tag} не содержит {nameof(Canvas)}");
                        return null;
                    }
                }

                return this.mainCanvas;
            }
        }

        public Canvas HudCanvas
        {
            get
            {
                if (this.hudCanvas == null)
                {
                    var tag = Constants.Tags.HudCanvas;
                    var gameObject = GameObject.FindWithTag(tag);
                    if (gameObject == null)
                    {
                        Debug.LogError($"Не найден объект с тегом {tag}");
                        return null;
                    }

                    this.hudCanvas = gameObject.GetComponent<Canvas>();
                    if (this.hudCanvas == null)
                    {
                        Debug.LogError($"Объект с тегом {tag} не содержит {nameof(Canvas)}");
                        return null;
                    }
                }

                return this.hudCanvas;
            }
        }
    }
}
