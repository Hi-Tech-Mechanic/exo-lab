namespace ExoLab.UI
{
    using Assets.PersonalAssets.Scripts.Inventory.View;
    using System;
    using UnityEngine;

    /// <summary>
    /// Вызывает <see cref="ItemInfoPanel"/> при наведении курсора на предмет
    /// </summary>
    public class ItemInfoSummoner : MonoBehaviour, ISubsribable
    {
        [SerializeField] private GameObject infoPanelPrefab;
        [SerializeField] private CharacterInventoryView characterInventory;
        [SerializeField] private AssemblyInventoryView assemblyInventory;

        private GameObject createdInfoPanel;
        private RectTransform? infoPanelRectTransform = null;
        private Vector2 canvasOffsetFromCursor = new Vector2(20F, -20F);
        private Vector2 worldSpaceOffsetFromCursor = new Vector2(-1200F, -760F);
        private bool panelInWorldSpace = false;

        /// <summary>
        /// Родитель всех созданных панелей и окон
        /// </summary>
        public Transform PanelsHolder 
        {
            get
            {
                var characterInventoryHandler = this.characterInventory.GetInfoPanelsHandler();

                if (characterInventoryHandler.activeInHierarchy)
                {
                    return characterInventoryHandler.transform;
                }

                var assemblyWindowHandler = this.assemblyInventory.GetInfoPanelsHandler();

                if (assemblyWindowHandler.activeInHierarchy)
                {
                    return assemblyWindowHandler.transform;
                }

                throw new Exception("No one of the inventories is included");
            }
        }

        private void OnEnable()
        {
            this.SubscribeEvents();
        }

        private void OnDisable()
        {
            this.UnsubscribeEvents();
        }

        public void SubscribeEvents()
        {
            GameEvents.UserEvents.OnItemHovered += ItemHoveredHandler;
            GameEvents.UserEvents.OnItemMoved += ItemMovedHandler;
            GameEvents.UserEvents.OnItemUnhovered += ItemUnhoveredHandler;
            GameEvents.UserEvents.OnItemClicked += ItemClickedHandler;
        }

        public void UnsubscribeEvents()
        {
            GameEvents.UserEvents.OnItemHovered -= ItemHoveredHandler;
            GameEvents.UserEvents.OnItemMoved -= ItemMovedHandler;
            GameEvents.UserEvents.OnItemUnhovered -= ItemUnhoveredHandler;
            GameEvents.UserEvents.OnItemClicked -= ItemClickedHandler;
        }

        private void ItemClickedHandler(ItemBase item)
        {
            this.InvokeRealItemInfoPanel(item);
        }

        private void ItemHoveredHandler(ItemBase item)
        {
            this.InvokeTooltipItemInfoPanel(item);
        }

        private void ItemUnhoveredHandler()
        {
            this.DeleteCreatedInfoPanel();
        }

        private void ItemMovedHandler()
        {
            if (infoPanelRectTransform == null)
            {
                return;
            }

            this.UpdatePosition();
        }

        /// <summary>
        /// Вызвать инфо панель в физичном окне, например по нажатию
        /// </summary>
        private void InvokeRealItemInfoPanel(ItemBase item)
        {
            var windowName = $"Характеристики - [{item.ItemData.Name}]";
            FloatingWindowsController.Instance.AddWindow(this.createdInfoPanel, windowName);
        }

        /// <summary>
        /// Создать плавающее окно подсказка
        /// </summary>
        private void InvokeTooltipItemInfoPanel(ItemBase item)
        {
            this.createdInfoPanel = Instantiate(this.infoPanelPrefab, this.PanelsHolder);
            
            this.panelInWorldSpace = this.createdInfoPanel.GetComponentInParent<Canvas>().renderMode == RenderMode.WorldSpace;

            this.infoPanelRectTransform = this.createdInfoPanel.GetComponent<RectTransform>();
            // Устанавливаем Anchor и Pivot в левый нижний угол для удобства расчетов
            this.infoPanelRectTransform.pivot = new Vector2(0, 0);

            var itemInfo = this.createdInfoPanel.GetComponent<ItemInfoPanel>();
            itemInfo.Initialize(item);
        }

        private void DeleteCreatedInfoPanel()
        {
            Destroy(this.createdInfoPanel);
            this.createdInfoPanel = default;
            this.infoPanelRectTransform = default;
            this.panelInWorldSpace = default;
        }

        private void UpdatePosition()
        {
            if (this.panelInWorldSpace)
            {
                this.UpdateWorldSpacePosition();
            }
            else
            {
                this.UpdateCanvasPosition();
            }
        }

        private void UpdateWorldSpacePosition()
        {
            Vector2 mousePosition = Input.mousePosition;

            // Рассчитываем желаемую позицию со смещением
            float targetX = mousePosition.x + (this.worldSpaceOffsetFromCursor.x);
            float targetY = mousePosition.y + (this.worldSpaceOffsetFromCursor.y);

            if (this.panelInWorldSpace)
            {
                this.createdInfoPanel.transform.localPosition = new Vector3(targetX, targetY, 0);
                return;
            }
        }

        private void UpdateCanvasPosition()
        {
            Vector2 mousePosition = Input.mousePosition;

            // Рассчитываем желаемую позицию со смещением
            float targetX = mousePosition.x + canvasOffsetFromCursor.x;
            float targetY = mousePosition.y + canvasOffsetFromCursor.y;

            if (this.panelInWorldSpace)
            {
                this.createdInfoPanel.transform.localPosition = new Vector3(targetX, targetY, 0);
                return;
            }

            // Учитываем размеры окна, чтобы понять, влезет ли оно
            float tooltipWidth = this.infoPanelRectTransform.rect.width * this.PanelsHolder.localScale.x;
            float tooltipHeight = this.infoPanelRectTransform.rect.height * this.PanelsHolder.localScale.y;

            // Проверка правой границы: если не влезает справа, перекидываем влево от мыши
            if (targetX + tooltipWidth > Screen.width)
            {
                targetX = mousePosition.x - tooltipWidth - canvasOffsetFromCursor.x;
            }

            // Проверка верхней границы: если не влезает сверху, уходим вниз
            if (targetY + tooltipHeight > Screen.height)
            {
                targetY = mousePosition.y - tooltipHeight - canvasOffsetFromCursor.y;
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
