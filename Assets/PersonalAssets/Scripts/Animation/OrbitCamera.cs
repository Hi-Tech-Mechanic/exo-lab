namespace Exolab
{
    using UnityEngine;
    using static ExoLab.Constants.Constants;

    public class OrbitCamera : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("Объект, вокруг которого крутимся")]
        [SerializeField] private Transform target;

        [Header("Rotation Limits")]
        [Tooltip("Угол вверх/вниз (в градусах)")]
        [SerializeField] private Vector2 pitchMinMax = new Vector2(-30f, 60f);
        [Tooltip("Угол влево/вправо (в градусах)")]
        [SerializeField] private Vector2 yawMinMax = new Vector2(-180f, 180f);

        [Header("Rotation Speed")]
        [Tooltip("Скорость вращения по горизонтали (градусы/сек)")]
        [SerializeField] private float yawSpeed = 10F;
        [Tooltip("Скорость вращения по вертикали")]
        [SerializeField] private float pitchSpeed = 10F;

        [Header("Zoom")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minCameraDistance = 1f;
        [SerializeField] private float maxCameraDistance = 5f;

        [Header("Smoothing")]
        [SerializeField] private bool smoothFollow = true;
        [SerializeField] private float smoothFollowTime = 0.1f;

        //[Header("Other")]
        //[SerializeField] private ItemInspect? itemInspect;

        private float currentYaw = 0f;
        private float currentPitch = 10f;

        private float currentCameraDistance;

        private bool yawDirection = true; // true = вправо, false = влево

        private void Awake()
        {
            this.currentYaw = transform.localRotation.y;
            this.currentCameraDistance = Mathf.Clamp((transform.position - target.position).magnitude, minCameraDistance, maxCameraDistance);
        }

        //private void OnEnable()
        //{
        //    if (itemInspect != null)
        //    {
        //        itemInspect.OnZoomChanged += UpdateDistance;
        //    }
        //}

        //private void OnDisable()
        //{
        //    if (itemInspect != null)
        //    {
        //        itemInspect.OnZoomChanged -= UpdateDistance;
        //    }
        //}

        private void LateUpdate()
        {
            if (target == null) 
                return;

            if (yawDirection)
            {
                currentYaw += yawSpeed * Time.deltaTime;
            }
            else
            {
                currentYaw -= yawSpeed * Time.deltaTime;
            }

            // Переключаем направление при достижении границ
            if (currentYaw >= yawMinMax.y)
            {
                currentYaw = yawMinMax.y;
                yawDirection = false;
            }
            else if (currentYaw <= yawMinMax.x)
            {
                currentYaw = yawMinMax.x;
                yawDirection = true;
            }

            currentPitch = Mathf.Clamp(currentPitch, pitchMinMax.x, pitchMinMax.y);

            SetZoomWithMouseScroll();

            // Вычисляем направление взгляда
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            Vector3 position = target.position - rotation * Vector3.forward * currentCameraDistance;

            // Применяем позицию и поворот
            if (smoothFollow)
            {
                transform.position = Vector3.Lerp(transform.position, position, smoothFollowTime);
                transform.LookAt(target);
            }
            else
            {
                transform.position = position;
                transform.LookAt(target);
            }
        }

        private void SetZoomWithMouseScroll()
        {
            float scroll = Input.GetAxis(InputAxes.MouseScrollWheel);
            if (scroll == 0f)
                return;

            this.currentCameraDistance -= scroll * this.zoomSpeed;
            this.currentCameraDistance = Mathf.Clamp(this.currentCameraDistance, this.minCameraDistance, this.maxCameraDistance);
        }

        //private void UpdateDistance(float value)
        //{
        //    this.distance = value;
        //}
    }
}
