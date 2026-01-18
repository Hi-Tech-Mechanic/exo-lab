using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    private const float UpdateInterval = 0.5f;
    private float lastUpdate;
    private int frameCount;
    private float fps;

    private GUIStyle style;
    private GUIContent content;
    private float width = 150f;
    private float height = 30f;

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.green;
        style.fontStyle = FontStyle.Bold;

        content = new GUIContent(); // переиспользуем объект для избежания аллокаций
    }

    void Update()
    {
        frameCount++;
        if (Time.realtimeSinceStartup - lastUpdate >= UpdateInterval)
        {
            fps = frameCount / (Time.realtimeSinceStartup - lastUpdate);
            frameCount = 0;
            lastUpdate = Time.realtimeSinceStartup;
        }
    }

    void OnGUI()
    {
        // Позиция: левый нижний угол
        float x = 10f;
        float y = Screen.height - height - 10f; // 10px отступ снизу

        var rect = new Rect(x, y, width, height);
        content.text = $"FPS: {fps:F0}";
        GUI.Label(rect, content, style);
    }
}