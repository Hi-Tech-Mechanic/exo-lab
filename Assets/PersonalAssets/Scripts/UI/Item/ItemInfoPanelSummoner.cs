namespace ExoLab.UI
{
    using ExoLab.Data;
    using ExoLab.StructuralСomponents;
    using UnityEngine;

    /// <summary>
    /// Вызывает <see cref="ItemInfoPanel"/> при наведении курсора на предмет
    /// </summary>
    public class ItemInfoPanelSummoner : HoverableElementAbstract
    {
        [SerializeField] private GameObject infoPanelPrefab;
        [SerializeField] private GameObject floatingWindow;

        private GameObject createdInfoPanel;
        private RectTransform infoPanelRectTransform;

        private Transform? _parentTransform;

        /// <summary>
        /// Родитель всех созданных панелей и окон
        /// </summary>
        private Transform parentTransform
        {
            get
            {
                if (this._parentTransform == null)
                {
                    this._parentTransform = Caches.Instance.Interface.MainCanvas.transform;
                }

                return this._parentTransform;
            }
        }

        protected override void ActionAfterClick()
        {
            this.InvokeFloatingWindow();
        }

        protected override void ActionAfterPointerEnter()
        {
            this.CreateItemInfoPanel();
            this.UpdatePosition();
        }

        protected override void ActionAfterPointerExit()
        {
            Destroy(this.createdInfoPanel);
            this.createdInfoPanel = default;
            this.infoPanelRectTransform = default;
        }

        protected override void ActionAfterPointerMove()
        {
            this.UpdatePosition();
        }

        /// <summary>
        /// Вызвать инфо панель в физичном окне, например по нажатию
        /// </summary>
        private void InvokeFloatingWindow()
        {
            var window = Instantiate(this.floatingWindow, this.parentTransform);
            var floatingWindow = window.GetComponent<FloatingWindow>();
            var assemblyComponent = this.GetComponent<AssemblyComponentBase>();

            var windowName = $"Характеристики - [{assemblyComponent.TypedItemData.Name}]";
            floatingWindow.InitializeWindow(this.createdInfoPanel, windowName);
        }

        private void CreateItemInfoPanel()
        {
            this.createdInfoPanel = Instantiate(this.infoPanelPrefab, this.parentTransform);
            this.infoPanelRectTransform = this.createdInfoPanel.GetComponent<RectTransform>();

            var assemblyComponent = this.GetComponent<AssemblyComponentBase>();
            var itemInfo = this.createdInfoPanel.GetComponent<ItemInfoPanel>();
            itemInfo.Initialize(assemblyComponent);
        }

        private void UpdatePosition()
        {
            var standardOffset_X = 10;
            //var offset_Y = this.rectTransform.position.y - this.rectTransform.sizeDelta.y;
            var offset = new Vector2((this.infoPanelRectTransform.sizeDelta.x / 2) + standardOffset_X, 0);

            Canvas parentCanvas = Caches.Instance.Interface.MainCanvas;
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();

            var leftOffset = new Vector2(Input.mousePosition.x - offset.x, Input.mousePosition.y - offset.y);
            var rightOffset = new Vector2(Input.mousePosition.x + offset.x, Input.mousePosition.y - offset.y);

            if (rightOffset.x >= Screen.width && rightOffset.x >= Input.mousePosition.x && leftOffset.x < Input.mousePosition.x)
            {
                this.infoPanelRectTransform.position = leftOffset;
            }
            else
            {
                this.infoPanelRectTransform.position = rightOffset;
            }

            // Ограничиваем позицию, чтобы окно не выходило за пределы экрана
            ClampToScreen();
        }

        private void ClampToScreen()
        {
            Canvas parentCanvas = Caches.Instance.Interface.MainCanvas;
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();

            // Получаем размеры окна
            Vector2 windowSize = infoPanelRectTransform.sizeDelta;

            // Получаем текущую позицию
            Vector2 anchoredPos = infoPanelRectTransform.anchoredPosition;

            // Определяем границы
            float clampedX = Mathf.Clamp(anchoredPos.x,
                -canvasRect.sizeDelta.x / 2 + windowSize.x / 2,
                canvasRect.sizeDelta.x / 2 - windowSize.x / 2);

            float clampedY = Mathf.Clamp(anchoredPos.y,
                -canvasRect.sizeDelta.y / 2 + windowSize.y / 2,
                canvasRect.sizeDelta.y / 2 - windowSize.y / 2);

            infoPanelRectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
        }
    }
}
