//using UnityEngine;
//using UnityEngine.UI; // Обязательно добавьте это
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;

//public class ScreenshotManager : MonoBehaviour
//{
//    public Camera screenshotCamera;
//    public Transform capturePoint;
//    public Image uiImageDisplay; // Ссылка на ваш UI Image
//    public int captureWidth = 512;
//    public int captureHeight = 512;

//    public string saveFolder = "ModelScreenshots";

//    // В этом примере мы захватываем только одну модель для отображения в UI
//    // Если вам нужно перебирать все модели, логика будет немного сложнее 
//    // (например, показывать их по очереди или сохранять в список спрайтов).

//    void Start()
//    {
//        // Запускаем процесс захвата первой модели при старте
//        StartCoroutine(CaptureAndDisplayFirstModelRoutine());
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Tab))
//            StartCoroutine(CaptureAndDisplayFirstModelRoutine());
//    }

//    IEnumerator CaptureAndDisplayFirstModelRoutine()
//    {
//        var modelsToCapture = GameObject.FindGameObjectsWithTag("Finish");

//        if (modelsToCapture.Length == 0)
//        {
//            Debug.LogWarning("Модели для захвата не найдены. Убедитесь, что они имеют тег 'Model'.");
//            yield break;
//        }

//        yield return null; // Ждем 1 кадр для рендера

//        // Захватываем изображение и получаем Texture2D
//        Texture2D screenshotTexture = CaptureTexture();

//        // Преобразуем Texture2D в Sprite и отображаем в UI
//        if (screenshotTexture != null && uiImageDisplay != null)
//        {
//            Sprite sprite = Sprite.Create(
//                screenshotTexture,
//                new Rect(0, 0, screenshotTexture.width, screenshotTexture.height),
//                new Vector2(0.5f, 0.5f) // Точка опоры спрайта (центр)
//            );

//            sprite.name = "test";
//            uiImageDisplay.sprite = sprite;
//            Debug.Log("Скриншот загружен в UI Image.");
//        }
//    }

//    Texture2D CaptureTexture()
//    {
//        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 32);
//        screenshotCamera.targetTexture = rt;

//        screenshotCamera.Render();

//        RenderTexture.active = rt;
//        Texture2D screenshot = new Texture2D(captureWidth, captureHeight, TextureFormat.ARGB32, false);
//        screenshot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
//        screenshot.Apply();

//        screenshotCamera.targetTexture = null;
//        RenderTexture.active = null;
//        Destroy(rt);

//        string filePath = Path.Combine(saveFolder, $"{this.name}.png");
//        File.WriteAllBytes(filePath, bytes);
//        return screenshot; // Возвращаем Texture2D
//    }
//}
