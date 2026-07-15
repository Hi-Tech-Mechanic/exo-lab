namespace ExoLab.Data
{
    using UnityEngine;
    using ExoLab.Constants;
    using ExoLab.UI;

    public class InterfaceCache
    {
        private Canvas mainCanvas;
        private Canvas hudCanvas;
        private NodeOptions nodeOptions;
        private OptionsUI _optionsUI;

        public NodeOptions NodeOptions
        {
            get
            {
                if (this.nodeOptions == null)
                {
                    this.nodeOptions = Resources.Load<NodeOptions>($"{Constants.GameResourcesPath.MainFolder}/Nodes/NodeOptions");
                }

                return this.nodeOptions;
            }
        }

        public OptionsUI OptionsUI
        {
            get
            {
                if (this._optionsUI == null)
                {
                    this._optionsUI = Resources.Load<OptionsUI>($"{Constants.GameResourcesPath.MainFolder}/UI/{nameof(OptionsUI)}");
                }

                return this._optionsUI;
            }
        }

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
