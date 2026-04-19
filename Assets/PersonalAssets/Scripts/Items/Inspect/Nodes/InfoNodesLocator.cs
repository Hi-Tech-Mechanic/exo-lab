namespace ExoLab.Interaction
{
    using ExoLab.Data;
    using UnityEngine;
    using static ExoLab.Constants.Constants;

    internal class InfoNodesLocator : MonoBehaviour
    {
        private const float rayLength = 100F;

        private new Camera camera;

        private LayerMask layerMask;

        private InfoNode? lastUsedInfoNode;

        private void Awake()
        {
            InitializeComponents();
        }

        private void OnEnable()
        {
            GameEvents.OnAssemblyModeEnabled += this.AssemblyModeHandler;
        }

        private void OnDisable()
        {
            GameEvents.OnAssemblyModeEnabled -= this.AssemblyModeHandler;
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
            if (this.camera != null)
                return;

            this.layerMask = LayerMask.GetMask(Layers.Component.ToString());
            this.camera = Caches.Instance.AssemblyCamera;
        }

        private void Update()
        {
            this.ProcessClickInput();
        }

        private void ProcessClickInput()
        {
            if (Input.GetMouseButtonDown(InputButtons.LeftMouseButton) == false)
            {
                return;
            }

            this.SelectObject();
        }

        private void SelectObject()
        {
            var infoNode = this.GetHitTarget();

            // Удаляем выделение с прошлой другой детали, не та что нажата сейчас
            if (this.lastUsedInfoNode != null && this.lastUsedInfoNode.Equals(infoNode) == false)
            {
                this.lastUsedInfoNode?.TryChangeObjectSelectState(false);
                this.lastUsedInfoNode = null;
            }

            if (infoNode != null)
            {
                this.lastUsedInfoNode = infoNode;
                this.lastUsedInfoNode.TryChangeObjectSelectState(true);
            }
            else
            {
                this.lastUsedInfoNode?.TryChangeObjectSelectState(false);
                this.lastUsedInfoNode = null;
            }
        }

        private InfoNode? GetHitTarget()
        {
            var ray = this.camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, rayLength, this.layerMask))
            {
                var infoNode = hit.transform.GetComponent<InfoNode>();
                return infoNode;
            }

            return null;
        }
    }
}
