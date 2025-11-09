namespace ExoLab.Assembly
{
    using UnityEngine;
    using static ExoLab.Constants.Constants;

    /// <summary>
    /// Отвечает за вращение и просмотр объекта
    /// </summary>
    public class ItemInspect : MonoBehaviour
    {
        private const int leftMouseButton = 0;

        [Header("Настройки вращения")]
        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 5f;
        [SerializeField] private Camera inspectCamera;

        private float currentDistance;
        private Vector3 defaultPosition;
        private bool isInspecting = false;

        private void Start()
        {
            if (inspectCamera == null)
            {
                Debug.LogError($"{nameof(ItemInspect)}: Camera component not found!");
                enabled = false;
                return;
            }

            currentDistance = inspectCamera.transform.localPosition.z;
            defaultPosition = transform.position;
        }

        private void Update()
        {
            //if (!isInspecting) 
            //    return;

            this.SetRotationWithMouse();

            this.SetZoomWithMouseScroll();
        }

        private void SetRotationWithMouse()
        {
            if (Input.GetMouseButton(leftMouseButton))
            {
                float mouseX = Input.GetAxis(InputAxes.MouseX) * rotationSpeed;
                float mouseY = Input.GetAxis(InputAxes.MouseY) * rotationSpeed;

                transform.Rotate(Vector3.up, -mouseX, Space.World);
                transform.Rotate(Vector3.right, mouseY, Space.World);

                //// Ограничение наклона (чтобы не переворачивалось)
                //Vector3 euler = transform.eulerAngles;
                //euler.x = Mathf.Clamp(euler.x, -60f, 60f);
                //transform.eulerAngles = euler;
            }
        }

        private void SetZoomWithMouseScroll()
        {
            float scroll = Input.GetAxis(InputAxes.MouseScrollWheel);
            if (scroll == 0f)
                return;

            currentDistance += scroll * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, -maxDistance, -minDistance);
            inspectCamera.transform.localPosition = new Vector3(0, 0, currentDistance);
        }

        /// <summary>
        /// Активировать/деактивировать режим осмотра
        /// </summary>
        /// <param name="inspect"></param>
        public void SetInspectMode(bool inspect)
        {
            isInspecting = inspect;
            if (!inspect)
            {
                // Сбросить позицию и зум
                transform.rotation = Quaternion.identity;
                transform.position = defaultPosition;
                currentDistance = maxDistance; // или начальное значение
                inspectCamera.transform.localPosition = new Vector3(0, 0, currentDistance);
            }
        }

        //public void OnWeaponSelected(WeaponData weapon)
        //{
        //    // Показать 3D-модель оружия
        //    weaponModel.SetActive(true);
        //    weaponModel.GetComponent<WeaponInspect>().SetInspectMode(true);

        //    // Отключить обычный UI инвентаря (опционально)
        //    inventoryUI.SetActive(false);
        //}

        //public void OnExitInspect()
        //{
        //    weaponModel.GetComponent<WeaponInspect>().SetInspectMode(false);
        //    inventoryUI.SetActive(true);
        //}
    }
}
