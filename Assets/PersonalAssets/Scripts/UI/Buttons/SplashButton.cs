namespace ExoLab.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using System.Collections;

    [RequireComponent(typeof(Image))]
    public class UISplashButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const string shaderPath = "CustomRenderTexture/SplashButton";

        public enum FillDirection
        {
            BottomToTop,
            TopToBottom,
            LeftToRight,
            RightToLeft
        }

        [Header("Настройки эффекта")]
        [SerializeField] private float fillDuration = 0.3f;
        [SerializeField] private Color ribbonColor = new Color(1f, 1f, 1f, 0.3f);
        [SerializeField] private float ribbonIntensity = 1.5f;
        [SerializeField] private float edgeSmoothness = 0.02f;
        [SerializeField] private FillDirection autoDirection = FillDirection.BottomToTop;
        [SerializeField] private bool useAutoDirection = true;

        private Image image;
        private Material material;
        private Coroutine fillCoroutine;
        private int currentDirection;

        private void Awake()
        {
            image = GetComponent<Image>();

            // Создаем материал из шейдера
            Shader shader = Shader.Find(shaderPath);
            if (shader != null)
            {
                material = new Material(shader);
                image.material = material;

                // Инициализируем начальные значения
                InitializeMaterial();
            }
            else
            {
                Debug.LogError($"Шейдер {shaderPath} не найден!");
            }
        }

        private void OnDisable()
        {
            this.StopFillCoroutine();

            // Возвращаем материал к первоначальному состоянию
            material.SetFloat("_FillAmount", 0f);
        }

        private void OnDestroy()
        {
            this.StopFillCoroutine();

            if (material != null)
            {
                Destroy(material);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Останавливаем предыдущую корутину если есть
            StopFillCoroutine();

            // Определяем направление заполнения
            if (useAutoDirection)
            {
                currentDirection = DetermineDirection(eventData);
            }
            else
            {
                currentDirection = (int)autoDirection;
            }

            // Устанавливаем направление в материале
            this.SetMaterialFillDirection(currentDirection);

            // Запускаем корутину заполнения
            fillCoroutine = StartCoroutine(FillIn());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.StopFillCoroutine();

            // Запускаем корутину очистки (в том же направлении)
            fillCoroutine = StartCoroutine(FillOut());
        }


        private void StopFillCoroutine()
        {
            if (fillCoroutine != null)
            {
                StopCoroutine(fillCoroutine);
            }
        }

        private void SetMaterialFillAmount(float value)
        {
            material.SetFloat("_FillAmount", value);
        }

        private void SetMaterialFillDirection(int value)
        {
            material.SetInt("_FillDirection", value);
        }

        private void InitializeMaterial()
        {
            if (material == null) return;

            this.SetMaterialFillAmount(0f);
            this.SetMaterialFillDirection((int)autoDirection);
            material.SetColor("_RibbonColor", ribbonColor);
            material.SetFloat("_FillIntensity", ribbonIntensity);
            material.SetFloat("_EdgeSmoothness", edgeSmoothness);
        }

        private int DetermineDirection(PointerEventData eventData)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            // Получаем позицию мыши относительно центра кнопки
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint
            );

            // Определяем с какой стороны пришел курсор
            float normalizedX = (localPoint.x + rectTransform.rect.width / 2) / rectTransform.rect.width;
            float normalizedY = (localPoint.y + rectTransform.rect.height / 2) / rectTransform.rect.height;

            // Находим ближайшую границу
            float distToLeft = normalizedX;
            float distToRight = 1 - normalizedX;
            float distToBottom = normalizedY;
            float distToTop = 1 - normalizedY;

            float minDist = Mathf.Min(distToLeft, distToRight, distToBottom, distToTop);

            if (minDist == distToBottom) return 0; // Bottom to Top
            if (minDist == distToTop) return 1;    // Top to Bottom
            if (minDist == distToLeft) return 2;   // Left to Right
            return 3;                              // Right to Left
        }

        private IEnumerator FillIn()
        {
            float elapsed = 0f;
            float currentFill = material.GetFloat("_FillAmount");

            while (elapsed < fillDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fillDuration;

                // Плавное заполнение (ease-out)
                float smoothT = 1 - Mathf.Pow(1 - t, 3);
                float newFill = Mathf.Lerp(currentFill, 1f, smoothT);

                material.SetFloat("_FillAmount", newFill);

                yield return null;
            }

            // Убеждаемся что достигли полного заполнения
            material.SetFloat("_FillAmount", 1f);
            fillCoroutine = null;
        }

        private IEnumerator FillOut()
        {
            float elapsed = 0f;
            float currentFill = material.GetFloat("_FillAmount");

            while (elapsed < fillDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fillDuration;

                // Плавная очистка (ease-in)
                float smoothT = Mathf.Pow(t, 3);
                float newFill = Mathf.Lerp(currentFill, 0f, smoothT);

                material.SetFloat("_FillAmount", newFill);

                yield return null;
            }

            // Убеждаемся что достигли нуля
            this.SetMaterialFillAmount(0f);
            fillCoroutine = null;
        }
    }
}
