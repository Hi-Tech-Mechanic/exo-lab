namespace ExoLab.UI
{
    using Assets.PersonalAssets.Scripts.UI.Base;
    using ExoLab.Data;
    using ExoLab.StructuralСomponents;
    using UnityEngine;

    /// <summary>
    /// Вызывает <see cref="ItemInfoPanel"/> при наведении курсора на предмет
    /// </summary>
    public class ItemInfoPanelSummoner : HoverableElementAbstract
    {
        [SerializeField] private GameObject infoPanelPrefab;

        private GameObject createdInfoPanel;
        private RectTransform? infoPanelRectTransform = null;
        private Vector2 offsetFromCursor = new Vector2(20F, 0);
        private RectTransform? _canvasRect;

        /// <summary>
        /// Родитель всех созданных панелей и окон
        /// </summary>
        private RectTransform canvasRect
        {
            get
            {
                if (this._canvasRect == null)
                {
                    this._canvasRect = Caches.Instance.Interface.MainCanvas.GetComponent<RectTransform>();
                }

                return this._canvasRect;
            }
        }

        protected override void ActionAfterClick()
        {
            this.InvokeFloatingWindow();
        }

        protected override void ActionAfterPointerEnter()
        {
            this.CreateItemInfoPanel();
            //this.UpdatePosition();
        }

        protected override void ActionAfterPointerExit()
        {
            Destroy(this.createdInfoPanel);
            this.createdInfoPanel = default;
            this.infoPanelRectTransform = default;
        }

        protected override void ActionAfterPointerMove()
        {
            if (infoPanelRectTransform == null)
                return;

            this.UpdatePosition2();
        }

        /// <summary>
        /// Вызвать инфо панель в физичном окне, например по нажатию
        /// </summary>
        private void InvokeFloatingWindow()
        {
            var assemblyComponent = this.GetComponent<AssemblyComponentBase>();
            var windowName = $"Характеристики - [{assemblyComponent.TypedItemData.Name}]";
            FloatingWindowsController.Instance.AddWindow(this.createdInfoPanel, windowName);
        }

        private void CreateItemInfoPanel()
        {
            this.createdInfoPanel = Instantiate(this.infoPanelPrefab, this.canvasRect.transform);
            this.infoPanelRectTransform = this.createdInfoPanel.GetComponent<RectTransform>();
            // Устанавливаем Anchor и Pivot в левый нижний угол для удобства расчетов
            this.infoPanelRectTransform.pivot = new Vector2(0, 0);

            var assemblyComponent = this.GetComponent<AssemblyComponentBase>();
            var itemInfo = this.createdInfoPanel.GetComponent<ItemInfoPanel>();
            itemInfo.Initialize(assemblyComponent);
        }
   
        private void UpdatePosition2()
        {
            Vector2 mousePos = Input.mousePosition;

            // Рассчитываем желаемую позицию со смещением
            float targetX = mousePos.x + offsetFromCursor.x;
            float targetY = mousePos.y + offsetFromCursor.y;

            // Учитываем размеры окна, чтобы понять, влезет ли оно
            float tooltipWidth = this.infoPanelRectTransform.rect.width * this.canvasRect.localScale.x;
            float tooltipHeight = this.infoPanelRectTransform.rect.height * this.canvasRect.localScale.y;

            // Проверка правой границы: если не влезает справа, перекидываем влево от мыши
            if (targetX + tooltipWidth > Screen.width)
            {
                targetX = mousePos.x - tooltipWidth - offsetFromCursor.x;
            }

            // Проверка верхней границы: если не влезает сверху, уходим вниз
            if (targetY + tooltipHeight > Screen.height)
            {
                targetY = mousePos.y - tooltipHeight - offsetFromCursor.y;
            }

            // Зажим: если окно всё равно не влезает (экран слишком мал), 
            // прижимаем его жестко к краям экрана
            targetX = Mathf.Clamp(targetX, 0, Screen.width - tooltipWidth);
            targetY = Mathf.Clamp(targetY, 0, Screen.height - tooltipHeight);

            // Применяем позицию
            this.infoPanelRectTransform.position = new Vector3(targetX, targetY, 0);
        }
    }
}
