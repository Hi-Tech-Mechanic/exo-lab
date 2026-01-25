using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using TMPro;
using System.Reflection;

/// <summary>
/// Асинхронный загрузчик сцен
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI tipsText;
    [SerializeField] private TextMeshProUGUI doneText;
    [SerializeField] private TextMeshProUGUI buildVersionText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image backgroundImage;
    private RectTransform backgroundImageRectTransform;

    private SceneLoaderOptions loaderOptions;
    private float textLoopFadeDuration = 2F;

    // Пришлось сделать так, потому что сцена моментально
    // загружалась и без предупреждения
    private float loadDuration = 15F;

    private void Awake()
    {
        this.InitializeComponents();
        this.InitializeLoaderOptions();
        this.DisplayBuildVersion();

        this.StartCoroutine(LoadMainSceneAsync());
        this.StartCoroutine(СyclicСhangeBackgrounds());
    }

    private void InitializeComponents()
    {
        this.backgroundImageRectTransform = backgroundImage.GetComponent<RectTransform>();
    }

    private void InitializeLoaderOptions()
    {
        this.loaderOptions = Resources.Load<SceneLoaderOptions>($"SceneLoaderOptions");
    }

    private void DisplayBuildVersion()
    {
        var buildVersion = $"Build V-[{Application.version}]";
        var unityVersion = $"Unity V-[{Application.unityVersion}]";
        this.buildVersionText.text = $"{buildVersion}\n{unityVersion}";
    }

    private IEnumerator LoadMainSceneAsync()
    {
        //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(this.loaderOptions.TargetSceneName);
        //asyncLoad.allowSceneActivation = false; // Запрещаем автоматический переход

        var time = 0F;
        var progress = 0F;
        
        while (progress < 1)
        {
            time += Time.unscaledDeltaTime;
            progress = Mathf.Clamp01(time / this.loadDuration);
            this.progressText.text = Mathf.RoundToInt(progress * 100F) + "%";
            this.progressBar.value = progress;
            yield return null;
        }

        this.doneText.text = "Для продолжения нажмите любую кнопку";
        this.doneText.alignment = TextAlignmentOptions.BottomGeoAligned;
        this.doneText.DOFade(0, this.textLoopFadeDuration).SetLoops(-1, LoopType.Yoyo);

        yield return null;

        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        
        SceneManager.LoadSceneAsync(this.loaderOptions.TargetSceneName);
    }

    private IEnumerator СyclicСhangeBackgrounds()
    {
        this.backgroundImage.color = new Vector4(0.9F, 0.9F, 0.9F, 0);
        var maxCount = this.loaderOptions.Backgrounds.Length - 1; 
        int currentBackground = Random.Range(0, maxCount);

        while (true)
        {
            var newSprite = this.loaderOptions.Backgrounds[currentBackground];
            if (currentBackground < maxCount)
            {
                currentBackground++;
            }
            else
            {
                currentBackground = 0;
            }

            this.backgroundImage.sprite = newSprite;
            this.backgroundImageRectTransform.sizeDelta = newSprite.border;

            this.backgroundImage.DOFade(1, this.loaderOptions.backgroundFadeDuration);
            yield return new WaitForSeconds(this.loaderOptions.backgroundLifeDuration);

            this.backgroundImage.DOFade(0, this.loaderOptions.backgroundFadeDuration);
            yield return new WaitForSeconds(this.loaderOptions.backgroundFadeDuration);
        }
    }
}