using UnityEngine;

public class MouseParallax : MonoBehaviour
{
    [Tooltip("Сила смещения: чем меньше, тем медленнее движется объект")]
    public float parallaxStrength = 0.02f;

    [Tooltip("Ограничение смещения по X и Y")]
    public Vector2 clampRange = new Vector2(0.5f, 0.5f);

    private Camera mainCamera;
    private Vector3 originalPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (mainCamera == null) return;

        // Получаем позицию мыши в мировых координатах на плоскости Z = 0 (или нужной глубине)
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));

        // Нормализуем позицию мыши к диапазону [-1, 1] относительно центра экрана
        float mouseX = (Input.mousePosition.x / Screen.width) * 2f - 1f;
        float mouseY = (Input.mousePosition.y / Screen.height) * 2f - 1f;

        // Применяем смещение с учётом силы параллакса
        Vector3 offset = new Vector3(mouseX * parallaxStrength, mouseY * parallaxStrength, 0f);

        // Ограничиваем смещение
        offset.x = Mathf.Clamp(offset.x, -clampRange.x, clampRange.x);
        offset.y = Mathf.Clamp(offset.y, -clampRange.y, clampRange.y);

        // Устанавливаем новую позицию
        transform.position = originalPosition + offset;
    }
}