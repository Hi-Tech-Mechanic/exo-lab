namespace ExoLab.Assembly
{
    using DG.Tweening;
    using ExoLab.Data;
    using ExoLab.Interaction;
    using ExoLab.UI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using static ExoLab.Constants.Constants;

    /// <summary>
    /// Отвечает за вращение и просмотр объекта
    /// </summary>
    public class ItemInspect : MonoBehaviour
    {
        private const float resetViewDuration = Timings.Millisecond_1000;

        [Header("Настройки")]
        [SerializeField] private Camera inspectCamera;
        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minCameraDistance = 1f;
        [SerializeField] private float maxCameraDistance = 5f;
        [SerializeField] private bool zoomEnabled = true;
        [SerializeField] private bool rotateByCoordinate_X = true;
        [SerializeField] private bool rotateByCoordinate_Y = true;
        [Tooltip("Режим поиска сталкиваемых объектов, через Physics.Raycast или GraphicRaycaster")]
        [SerializeField] private bool useGraphicRaycaster = true;

        private float currentCameraDistance;
        private Quaternion defaultRotation;
        private Vector3 defaultPosition;

        private bool isInspecting = true;
        private bool rotationIsBlocked = false;

        private GraphicRaycaster raycaster;
        private PointerEventData pointerData;
        private EventSystem eventSystem = EventSystem.current;

        public float DefaultCameraDistance { get; set; }

        public event Action<Quaternion> OnRotationChanged;
        public event Action<float> OnZoomChanged;

        private void Awake()
        {
            this.Initialize();
        }

        private void OnEnable()
        {
            DraggableInventoryItem.OnBeginDragAction += this.DisableInspectMode;
            DraggableInventoryItem.OnEndDragAction += this.EnableInspectMode;
            InteractiveIKController.OnItemInspectRotationBlock += this.SetRotationBlockState;
        }

        private void OnDisable()
        {
            DraggableInventoryItem.OnBeginDragAction -= this.DisableInspectMode;
            DraggableInventoryItem.OnEndDragAction -= this.EnableInspectMode;
            InteractiveIKController.OnItemInspectRotationBlock -= this.SetRotationBlockState;
        }

        private void Update()
        {
            if (this.isInspecting == false)
                return;

            if (CursorInAssemblyZone() == false)
                return;

            this.SetRotationWithMouse();
            this.SetZoomWithMouseScroll();
        }

        /// <summary>
        /// Вернуть отображение в изначальное состояние
        /// </summary>
        public void ResetToDefaultView()
        {
            this.transform.DOLocalMove(this.defaultPosition, resetViewDuration);
            this.transform.DOLocalRotate(this.defaultRotation.eulerAngles, resetViewDuration);
            this.inspectCamera.transform.DOLocalMoveZ(this.DefaultCameraDistance, resetViewDuration);
        }

        /// <summary>
        /// Активировать/деактивировать режим осмотра
        /// </summary>
        /// <param name="inspect"></param>
        public void ToggleInspectMode()
        {
            this.isInspecting = !this.isInspecting;
        }

        private void Initialize()
        {
            this.raycaster = Caches.Instance.Interface.MainCanvas.GetComponent<GraphicRaycaster>();
            this.pointerData = new PointerEventData(eventSystem);

            if (this.inspectCamera == null)
            {
                Debug.LogError($"{nameof(ItemInspect)}: Camera component not found!");
                this.enabled = false;
                return;
            }

            this.currentCameraDistance = this.inspectCamera.transform.localPosition.z;
            this.DefaultCameraDistance = this.currentCameraDistance;
            this.defaultPosition = this.transform.localPosition;
            this.defaultRotation = this.transform.localRotation;
        }

        private bool CursorInAssemblyZone()
        {
            if (this.useGraphicRaycaster)
            {
                var results = new List<RaycastResult>();

                this.pointerData.position = Input.mousePosition;
                this.raycaster.Raycast(this.pointerData, results);

                // Если есть что-то кроме целевого объекта или вообще нет
                if (results.Count == 0 || results.Count > 1)
                    return false;

                var inZone = results.Any(target => target.gameObject.tag == Tags.AssemblyZone);

                return inZone;
            }
            else
            {
                var ray = this.inspectCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit) == false)
                    return false;

                if (hit.collider.gameObject.tag == Tags.AssemblyZone)
                    return true;
            }

            return false;
        }

        private void SetRotationWithMouse()
        {
            var mouseLeftClicked = Input.GetMouseButton(InputButtons.LeftMouseButton);
            var mouseRightClicked = Input.GetMouseButton(InputButtons.RightMouseButton);

            // Разрешаем вращать правой кнопкой мышки, даже если блокировка вращения включена
            if (this.rotationIsBlocked && mouseRightClicked == false)
                return;

            if (mouseLeftClicked == false && mouseRightClicked == false)
                return;

            if (this.rotateByCoordinate_X)
            {
                float mouseX = Input.GetAxis(InputAxes.MouseX) * this.rotationSpeed;
                this.transform.Rotate(Vector3.up, -mouseX, Space.World);
            }
            if (this.rotateByCoordinate_Y)
            {
                float mouseY = Input.GetAxis(InputAxes.MouseY) * this.rotationSpeed;
                this.transform.Rotate(Vector3.right, mouseY, Space.World);
            }

            this.OnRotationChanged?.Invoke(this.transform.rotation);
        }

        private void SetZoomWithMouseScroll()
        {
            if (this.zoomEnabled == false)
                return;

            float scroll = Input.GetAxis(InputAxes.MouseScrollWheel);
            if (scroll == 0f)
                return;

            this.currentCameraDistance += scroll * this.zoomSpeed;
            this.currentCameraDistance = Mathf.Clamp(this.currentCameraDistance, -this.maxCameraDistance, -this.minCameraDistance);
            this.inspectCamera.transform.localPosition = new Vector3(0, 0, this.currentCameraDistance);

            this.OnZoomChanged?.Invoke(this.currentCameraDistance);
        }

        private void DisableInspectMode()
        {
            this.isInspecting = false;
        }

        private void EnableInspectMode()
        {
            this.isInspecting = true;
        }

        private void SetRotationBlockState(bool value) 
        {
            this.rotationIsBlocked = value;
        }
    }
}
