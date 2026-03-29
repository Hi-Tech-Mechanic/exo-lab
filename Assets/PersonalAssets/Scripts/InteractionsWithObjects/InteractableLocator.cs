namespace ExoLab.Interaction
{
    using ExoLab.Data;
    using ExoLab.Input;
    using UnityEngine;

    /// <summary>
    /// Отвечает за поиск интерактивных объектов, 
    /// в основании которых лежит <see cref="InteractiveObject"/>
    /// </summary>
    internal class InteractableLocator : MonoBehaviour
    {
        private const float maxDistance = 2F;

        private LayerMask interactableLayer;
        private LayerMask componentLayer;
        private Camera camera;

        /// <summary>
        /// Храним временно объект на который смотрим
        /// </summary>
        private InteractiveObject? hoveredObject;

        private void Awake()
        {
            this.Init();
        }

        private void OnEnable()
        {
            InputControllersManager.Instance.Interaction.OnInteractPressed += Interact;
        }

        private void OnDisable()
        {
            InputControllersManager.Instance.Interaction.OnInteractPressed -= Interact;
        }

        private void Update()
        {
            if (this.TryFindInteractableObject(out var localHoveredObject))
            {
                this.hoveredObject = localHoveredObject;
                this.hoveredObject?.DisplayMessage();
            }
            else
            {
                if (this.hoveredObject == null)
                    return;

                this.hoveredObject?.HideMessage();
                this.hoveredObject = null;
            }
        }

        private void Interact()
        {
            if (this.hoveredObject == null)
                return;

            this.hoveredObject?.Interact();
        }

        private void Init()
        {
            this.camera = Caches.Instance.MainCamera;
            this.interactableLayer = LayerMask.GetMask(Constants.Constants.Layers.Interactable.ToString());
            this.componentLayer = LayerMask.GetMask(Constants.Constants.Layers.Component.ToString());
        }
        
        private bool TryFindInteractableObject(out InteractiveObject? hoveredObject)
        {
            RaycastHit hit;
            hoveredObject = null;

            // Пока через камеру лучше все таки работает
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            //var ray = new Ray(this.transform.position, transform.forward);

            // Проверяем 2 слоя так как у компонентов уже задан слой и его не желательно менять
            if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer) == false &&
                Physics.Raycast(ray, out hit, maxDistance, componentLayer) == false)
            {
                return false;
            }

            hoveredObject = hit.transform.GetComponent<InteractiveObject>();
            return true;
        }
    }
}
