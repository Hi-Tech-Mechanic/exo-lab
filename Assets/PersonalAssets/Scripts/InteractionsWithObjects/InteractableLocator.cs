namespace ExoLab.Interaction
{
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Отвечает за поиск интерактивных объектов, 
    /// в основании которых лежит <see cref="InteractiveObject"/>
    /// </summary>
    internal class InteractableLocator : MonoBehaviour
    {
        private const float maxDistance = 10F;

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

        private void Update()
        {
            if (this.TryFindInteractableObject())
            {
                this.hoveredObject?.DisplayMessage();

                if (Input.GetKeyDown(KeyCode.E) == true)
                {
                    this.hoveredObject?.Interact();
                }
            }
            else
            {
                this.hoveredObject?.HideMessage();
                this.hoveredObject = null;
            }
        }

        private void Init()
        {
            this.camera = Caches.Instance.MainCamera;
            this.interactableLayer = LayerMask.GetMask(Constants.Constants.Layers.Interactable.ToString());
            this.componentLayer = LayerMask.GetMask(Constants.Constants.Layers.Component.ToString());
        }
        
        private bool TryFindInteractableObject()
        {
            RaycastHit hit;

            // Пока через камеру лучше все таки работает
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            //var ray = new Ray(this.transform.position, transform.forward);

            // Проверяем 2 слоя так как у компонентов уже задан слой и его не желательно менять
            if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer) == false &&
                Physics.Raycast(ray, out hit, maxDistance, componentLayer) == false)
            {
                return false;
            }

            this.hoveredObject = hit.transform.GetComponent<InteractiveObject>();
            return true;
        }
    }
}
