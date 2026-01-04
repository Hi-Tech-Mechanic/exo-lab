namespace ExoLab.Assembly
{
    using ExoLab.Data;
    using UnityEngine;
    using static ExoLab.Constants.Constants;

    /// <summary>
    /// Отвечает за вращение и просмотр объекта
    /// </summary>
    public class ItemInspect : MonoBehaviour
    {
        private const int maxRayCastDistance = 100;

        [Header("Настройки вращения")]
        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 5f;
        [SerializeField] private Camera inspectCamera;

        private float currentDistance;
        private Vector3 defaultPosition;
        //private bool isInspecting = false;

        private LayerMask interactableInspectLayer;

        private void Awake()
        {
            this.Initialize();
        }

        private void Update()
        {
            //if (this.isInspecting == false)
            //    return;

            this.SetRotationWithMouse();

            this.SetZoomWithMouseScroll();
        }

        private void Initialize()
        {
            if (inspectCamera == null)
            {
                Debug.LogError($"{nameof(ItemInspect)}: Camera component not found!");
                this.enabled = false;
                return;
            }

            this.currentDistance = this.inspectCamera.transform.localPosition.z;
            this.defaultPosition = this.transform.position;

            interactableInspectLayer = LayerMask.GetMask(Layers.UI);
        }

        private void SetRotationWithMouse()
        {
            if (Input.GetMouseButton(InputButtons.leftMouseButton) == false)
                return;

            Ray ray = Caches.Instance.AssemblyCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out var hit, maxRayCastDistance, this.interactableInspectLayer))
            {
                // hit.collider.gameObject — объект, в который попали
                Debug.Log("Попали в: " + hit.collider.gameObject.name);

                // Можно проверить тег, слой, компонент и т.д.
                if (hit.collider.CompareTag(Tags.AssemblyZone))
                {
                    Debug.Log("Это враг!");
                    // Делаем что-то с врагом
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Это игрок!");
                }

                float mouseX = Input.GetAxis(InputAxes.MouseX) * this.rotationSpeed;
                float mouseY = Input.GetAxis(InputAxes.MouseY) * this.rotationSpeed;

                this.transform.Rotate(Vector3.up, -mouseX, Space.World);
                this.transform.Rotate(Vector3.right, mouseY, Space.World);
            }


            // Или использовать switch/case по имени или тегу

            //// Ограничение наклона (чтобы не переворачивалось)
            //Vector3 euler = transform.eulerAngles;
            //euler.x = Mathf.Clamp(euler.x, -60f, 60f);
            //transform.eulerAngles = euler;
        }

        private void SetZoomWithMouseScroll()
        {
            float scroll = Input.GetAxis(InputAxes.MouseScrollWheel);
            if (scroll == 0f)
                return;

            this.currentDistance += scroll * this.zoomSpeed;
            this.currentDistance = Mathf.Clamp(currentDistance, -maxDistance, -minDistance);
            this.inspectCamera.transform.localPosition = new Vector3(0, 0, this.currentDistance);
        }

        /// <summary>
        /// Активировать/деактивировать режим осмотра
        /// </summary>
        /// <param name="inspect"></param>
        public void ToggleInspectMode()
        {
            //this.isInspecting = !this.isInspecting;
            //if (this.isInspecting == false)
            //{
            //    // Сбросить позицию и зум
            //    //this.transform.rotation = Quaternion.identity;
            //    //this.transform.position = defaultPosition;
            //}
        }

        //public void OnWeaponSelected(WeaponData weapon)
        //{
        //    // Показать 3D-модель оружия
        //    weaponModel.SetActive(true);
        //    weaponModel.GetComponent<WeaponInspect>().ToggleInspectMode(true);

        //    // Отключить обычный UI инвентаря (опционально)
        //    inventoryUI.SetActive(false);
        //}

        //public void OnExitInspect()
        //{
        //    weaponModel.GetComponent<WeaponInspect>().ToggleInspectMode(false);
        //    inventoryUI.SetActive(true);
        //}
    }
}
