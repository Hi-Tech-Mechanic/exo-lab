namespace ExoLab.Interaction
{
    using ExoLab.Data;
    using ExoLab.UI;
    using UnityEngine;

    /// <summary>
    /// Отрисовщик ноды характеристик с указателем линии безье на выделенный объект
    /// </summary>
    [RequireComponent(typeof(InfoNodeController))]
    public class InfoNode : MonoBehaviour
    {
        private Vector2 currentOffset;
        private Vector3 currentScale;
        
        private GameObject? currentNodeWindow = null;
        private GameObject nodeWindowPrefab;
        private RectTransform nodeWindowRect;

        private Material defaultMaterial;
        private Material selectedStateMaterial;
        private Renderer currentRenderer;

        private NodeLine line;

        private bool infoNodeInitialized;

        public Vector2 BaseOffset { get; private set; }

        public Vector2 CurrentOffset 
        {
            get => this.currentOffset;
            set
            {   
                this.currentOffset = value;
                this.UpdatePosition();
            }
        }
        
        public Vector3 CurrentScale
        {
            get => this.currentScale;
            set
            {
                this.currentScale = value;

                if (this.currentNodeWindow != null)
                    this.currentNodeWindow.transform.localScale = this.CurrentScale;
            }
        }

        private void Awake()
        {
            this.InitializeComponents();
        }

        private void OnEnable()
        {
            GameEvents.OnAssemblyModeEnabled += this.AssemblyModeHandler;
        }

        private void OnDisable()
        {
            GameEvents.OnAssemblyModeEnabled -= this.AssemblyModeHandler;
        }

        public void TryChangeObjectSelectState(bool selected)
        {
            if (this.infoNodeInitialized == false)
            {
                Debug.Log($"[{this.name}]: не инициализирован");
                return;
            }

            if (selected)
            {
                this.PaintSelectedObject();
                this.OpenWindow();
            }
            else
            {
                if (this.currentNodeWindow == null)
                    return;

                this.PaintUnselectedObject();
                this.CloseWindow();
            }
        }

        private void AssemblyModeHandler(bool assemblyEnabled)
        {
            if (assemblyEnabled)
            {
                this.InitializeComponents();
            }
        }

        private void InitializeComponents()
        {
            if (this.infoNodeInitialized) // Проверка инициализирован ли уже
                return;

            var options = Caches.Instance.Interface.NodeOptions;

            this.BaseOffset = options.BaseOffset;
            this.CurrentOffset = options.BaseOffset;
            this.nodeWindowPrefab = options.WindowPrefab;

            this.selectedStateMaterial = options.SelectedStateMaterial;
            this.currentRenderer = this.GetComponent<Renderer>();
            this.defaultMaterial = this.currentRenderer.material;

            this.infoNodeInitialized = true;
        }

        private void UpdatePosition()
        {
            if (this.currentNodeWindow == null)
                return;

            var worldPosition = this.transform.position;
            Vector2 screenPoint = Caches.Instance.AssemblyCamera.WorldToScreenPoint(worldPosition);
            var parentRect = nodeWindowRect.parent as RectTransform;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPoint, null, out Vector2 localPoint))
            {
                // Умножил на 2 так как при центральном выравнивании окна смещение было невероятно маленьким
                this.nodeWindowRect.anchoredPosition = localPoint + (this.currentOffset * 2);
            }

            // StartPoint — центр 3D объекта
            this.line.startPoint = this.line.transform.InverseTransformPoint(screenPoint);

            // EndPoint — край окна (делаем локальными относительно объекта линии)
            // GetWorldCorners заполняет массив: [0] низо-лево, [1] верхо-лево, [2] верхо-право, [3] низо-право
            var corners = new Vector3[4];
            this.nodeWindowRect.GetWorldCorners(corners);
            var bottomLeftCorner = corners[0];
            this.line.endPoint = this.line.transform.InverseTransformPoint(bottomLeftCorner);
        }

        private void PaintSelectedObject()
        {
            var renderer = this.GetComponent<Renderer>();

            if (this.selectedStateMaterial.Equals(renderer.material) == false)
            {
                renderer.material = this.selectedStateMaterial;
            }
        }

        private void PaintUnselectedObject()
        {
            var renderer = this.GetComponent<Renderer>();

            if (this.defaultMaterial.Equals(renderer.material) == false)
            {
                renderer.material = this.defaultMaterial;
            }
        }

        private void OpenWindow()
        {
            if (this.currentNodeWindow == null)
            {
                this.currentNodeWindow = Instantiate(this.nodeWindowPrefab, Caches.Instance.Interface.HudCanvas.transform);
                var itemBase = this.GetComponent<ItemBase>();
                var itemInfo = this.currentNodeWindow.GetComponent<ItemInfoPanel>();
                itemInfo.Initialize(itemBase);

                this.line = this.currentNodeWindow.GetComponentInChildren<NodeLine>();
                this.nodeWindowRect = this.currentNodeWindow.GetComponent<RectTransform>();
                this.CurrentScale = this.nodeWindowRect.localScale;
            }
        }

        private void CloseWindow()
        {
            if (this.currentNodeWindow != null)
            {
                Destroy(this.currentNodeWindow.gameObject);
            }
        }
    }
}
