namespace ExoLab.UI
{
    using ExoLab.Data;
    using ExoLab.StructuralСomponents;
    using UnityEngine;
    using static ExoLab.Constants.Constants;

    /// <summary>
    /// Отрисовщик ноды характеристик с указателем линии безье на выделенный объект
    /// </summary>
    [RequireComponent(typeof(NodeLayoutController))]
    public class NodeInfoPopup : MonoBehaviour
    {
        private Vector2 currentOffset;
        private Vector3 currentScale;
        
        private GameObject? currentWindow = null;
        private RectTransform windowRect;
        private NodeLine line;

        private bool windowIsEnabled = false;
        private bool initialized = false;
        private Material defaultMaterial;
        private Renderer currentRenderer;

        public NodeOptions NodeOptions { get; set; }

        public Vector2 BaseOffset { get; set; }

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

                if (this.currentWindow != null)
                    this.currentWindow.transform.localScale = this.CurrentScale;
            }
        }

        private void Awake()
        {
            this.InitializeComponents();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(InputButtons.LeftMouseButton))
            {
                var ray = Caches.Instance.AssemblyCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.transform.name == this.transform.name)
                    {
                        this.PaintSelectedObject();
                        this.OpenWindow();
                    }
                    else
                    {
                        if (windowIsEnabled == false)
                            return;

                        this.PaintUnselectedObject();
                        this.CloseWindow();
                    }
                }
            }
        }

        public void InitializeComponents()
        {
            if (this.initialized)
                return;

            this.NodeOptions = Caches.Instance.Interface.NodeOptions;

            this.BaseOffset = this.NodeOptions.BaseOffset;
            this.CurrentOffset = this.BaseOffset;
            this.currentRenderer = this.GetComponent<Renderer>();
            this.defaultMaterial = this.currentRenderer.material;

            this.initialized = true;
        }

        private void UpdatePosition()
        {
            if (this.windowIsEnabled == false)
                return;

            if (this.currentWindow == null)
                return;

            var worldPosition = this.transform.position;
            Vector2 screenPoint = Caches.Instance.AssemblyCamera.WorldToScreenPoint(worldPosition);
            var parentRect = windowRect.parent as RectTransform;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPoint, null, out Vector2 localPoint))
            {
                // Умножил на 2 так как при центральном выравнивании окна смещение было невероятно маленьким
                this.windowRect.anchoredPosition = localPoint + (this.currentOffset * 2);
            }

            // StartPoint — центр 3D объекта
            this.line.startPoint = this.line.transform.InverseTransformPoint(screenPoint);

            // EndPoint — край окна (делаем локальными относительно объекта линии)
            // GetWorldCorners заполняет массив: [0] низо-лево, [1] верхо-лево, [2] верхо-право, [3] низо-право
            var corners = new Vector3[4];
            this.windowRect.GetWorldCorners(corners);
            var bottomLeftCorner = corners[0];
            this.line.endPoint = this.line.transform.InverseTransformPoint(bottomLeftCorner);
        }

        private void PaintSelectedObject()
        {
            GetComponent<Renderer>().material = this.NodeOptions.SelectedStateMaterial;
        }

        private void PaintUnselectedObject()
        {
            GetComponent<Renderer>().material = this.defaultMaterial;
        }

        private void OpenWindow()
        {
            if (this.currentWindow == null)
            {
                this.windowIsEnabled = true;

                this.currentWindow = Instantiate(this.NodeOptions.WindowPrefab, Caches.Instance.Interface.HudCanvas.transform);
                var assemblyComponent = this.GetComponent<AssemblyComponentBase>();
                var itemInfo = this.currentWindow.GetComponent<ItemInfoPanel>();
                itemInfo.Initialize(assemblyComponent);

                this.line = this.currentWindow.GetComponentInChildren<NodeLine>();
                this.windowRect = this.currentWindow.GetComponent<RectTransform>();
                this.CurrentScale = this.windowRect.localScale;
            }
        }

        private void CloseWindow()
        {
            if (this.currentWindow != null)
            {
                this.windowIsEnabled = false;
                Destroy(this.currentWindow.gameObject);
            }
        }
    }
}
