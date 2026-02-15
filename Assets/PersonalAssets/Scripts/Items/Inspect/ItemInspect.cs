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

        /// <summary>
        /// Перечень передаваемых настроек, так как режимов может быть несколько,
        /// а экземпляр <see cref="ItemInspect"/> должен быть 1 на сцене
        /// </summary>
        public ItemInspectOptions ItemInspectOptions { private get; set; }

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
        public event Action OnCameraPositionChanged;

        private void Start()
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
            this.SetCameraPosition();
        }

        /// <summary>
        /// Вернуть отображение в изначальное состояние
        /// </summary>
        public void ResetToDefaultView()
        {
            this.ItemInspectOptions.TargetTransform.DOLocalMove(this.defaultPosition, resetViewDuration);
            this.ItemInspectOptions.TargetTransform.DOLocalRotate(this.defaultRotation.eulerAngles, resetViewDuration);
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

        /// <summary>
        /// Обновить поля
        /// </summary>
        public void UpdateOptions()
        {
            this.defaultPosition = this.ItemInspectOptions.TargetTransform.localPosition;
            this.defaultRotation = this.ItemInspectOptions.TargetTransform.localRotation;
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
            this.UpdateOptions();
        }

        private bool CursorInAssemblyZone()
        {
            if (this.ItemInspectOptions.UseGraphicRaycaster)
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

            if (this.ItemInspectOptions.RotateByCoordinate_X)
            {
                float mouseX = Input.GetAxis(InputAxes.MouseX) * this.ItemInspectOptions.RotationSpeed;
                this.ItemInspectOptions.TargetTransform.Rotate(Vector3.up, -mouseX, Space.World);
            }
            if (this.ItemInspectOptions.RotateByCoordinate_Y)
            {
                float mouseY = Input.GetAxis(InputAxes.MouseY) * this.ItemInspectOptions.RotationSpeed;
                this.ItemInspectOptions.TargetTransform.Rotate(Vector3.right, mouseY, Space.World);
            }

            this.OnRotationChanged?.Invoke(this.ItemInspectOptions.TargetTransform.rotation);
        }

        private void SetZoomWithMouseScroll()
        {
            if (this.ItemInspectOptions.ZoomEnabled == false)
                return;

            float scroll = Input.GetAxis(InputAxes.MouseScrollWheel);
            if (scroll == 0f)
                return;

            this.currentCameraDistance += scroll * this.ItemInspectOptions.ZoomSpeed;
            this.currentCameraDistance = Mathf.Clamp(this.currentCameraDistance, -this.ItemInspectOptions.MaxCameraDistance, -this.ItemInspectOptions.MinCameraDistance);
            this.inspectCamera.transform.localPosition = new Vector3(this.inspectCamera.transform.localPosition.x, this.inspectCamera.transform.localPosition.y, this.currentCameraDistance);

            this.OnZoomChanged?.Invoke(this.currentCameraDistance);
        }

        private void SetCameraPosition()
        {
            var mouseWheelClicked =  Input.GetMouseButton(InputButtons.MiddleMouseButton);
            if (mouseWheelClicked == false)
                return;

            var position = this.inspectCamera.transform.position;

            float mouseX = Input.GetAxis(InputAxes.MouseX);
            float mouseY = Input.GetAxis(InputAxes.MouseY);

            var targetPosition = new Vector3(-mouseX, -mouseY, position.z);
            this.inspectCamera.transform.localPosition = Vector3.Lerp(this.inspectCamera.transform.localPosition, targetPosition, 0.02f);

            this.OnCameraPositionChanged?.Invoke();
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
