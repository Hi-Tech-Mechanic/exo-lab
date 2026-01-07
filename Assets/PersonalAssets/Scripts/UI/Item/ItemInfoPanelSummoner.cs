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

        private GameObject infoPanelObject;
        private RectTransform infoPanelRectTransform;

        protected override void ActionAfterClick()
        {
            // todo Открыть окно полностью
            return;
        }

        protected override void ActionAfterPointerEnter()
        {
            this.CreateItemInfoPanel();
            this.UpdatePosition();
        }

        protected override void ActionAfterPointerExit()
        {
            Destroy(this.infoPanelObject);
            this.infoPanelObject = default;
            this.infoPanelRectTransform = default;
        }

        protected override void ActionAfterPointerMove()
        {
            this.UpdatePosition();
        }

        private void CreateItemInfoPanel()
        {
            var parentTransform = Caches.Instance.Interface.MainCanvas.transform;
            this.infoPanelObject = Instantiate(this.infoPanelPrefab, parentTransform);
            this.infoPanelRectTransform = this.infoPanelObject.GetComponent<RectTransform>();

            var assemblyComponent = this.gameObject.GetComponent<AssemblyComponentBase>();
            var itemInfo = this.infoPanelObject.GetComponent<ItemInfoPanel>();
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
