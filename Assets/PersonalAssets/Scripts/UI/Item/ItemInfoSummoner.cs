namespace ExoLab.UI
{
    using ExoLab.Data;
    using ExoLab.StructuralСomponents;
    using UnityEngine;

    public class ItemInfoSummoner : HoverableElementAbstract
    {
        [SerializeField]
        private GameObject itemInfoInspectorPrefab;

        private GameObject itemInfoMenu;
        private RectTransform itemInfoRectTransform;

        private RectTransform rectTransform;

        private void Awake()
        {
            this.rectTransform = this.gameObject.GetComponent<RectTransform>();
        }

        protected override void ActionAfterClick()
        {
            // todo Открыть окно полностью
            return;
        }

        protected override void ActionAfterPointerEnter()
        {
            this.itemInfoMenu = Instantiate(this.itemInfoInspectorPrefab, Caches.Instance.Interface.MainCanvas.transform);
            this.itemInfoRectTransform = this.itemInfoMenu.GetComponent<RectTransform>();
            UpdatePosition();

            var assemblyComponent = this.gameObject.GetComponent<AssemblyComponentBase>();
            var itemInfo = this.itemInfoMenu.GetComponent<ItemInfoInspector>();
            itemInfo.Initialize(assemblyComponent);
        }

        protected override void ActionAfterPointerExit()
        {
            Destroy(this.itemInfoMenu);
            this.itemInfoMenu = default;
            this.itemInfoRectTransform = default;
        }

        protected override void ActionAfterPointerMove()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var standardOffset_X = 10;
            //var offset_Y = this.rectTransform.position.y - this.rectTransform.sizeDelta.y;
            var offset = new Vector2((this.itemInfoRectTransform.sizeDelta.x / 2) + standardOffset_X, 0);

            Canvas parentCanvas = Caches.Instance.Interface.MainCanvas;
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();

            var leftOffset = new Vector2(Input.mousePosition.x - offset.x, Input.mousePosition.y - offset.y);
            var rightOffset = new Vector2(Input.mousePosition.x + offset.x, Input.mousePosition.y - offset.y);

            if (rightOffset.x >= Screen.width && rightOffset.x >= Input.mousePosition.x && leftOffset.x < Input.mousePosition.x)
            {
                this.itemInfoRectTransform.position = leftOffset;
            }
            else
            {
                this.itemInfoRectTransform.position = rightOffset;
            }

            // Ограничиваем позицию, чтобы окно не выходило за пределы экрана
            ClampToScreen();
        }

        private void ClampToScreen()
        {
            Canvas parentCanvas = Caches.Instance.Interface.MainCanvas;
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();

            // Получаем размеры окна
            Vector2 windowSize = itemInfoRectTransform.sizeDelta;

            // Получаем текущую позицию
            Vector2 anchoredPos = itemInfoRectTransform.anchoredPosition;

            // Определяем границы
            float clampedX = Mathf.Clamp(anchoredPos.x,
                -canvasRect.sizeDelta.x / 2 + windowSize.x / 2,
                canvasRect.sizeDelta.x / 2 - windowSize.x / 2);

            float clampedY = Mathf.Clamp(anchoredPos.y,
                -canvasRect.sizeDelta.y / 2 + windowSize.y / 2,
                canvasRect.sizeDelta.y / 2 - windowSize.y / 2);

            itemInfoRectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
        }
    }
}
