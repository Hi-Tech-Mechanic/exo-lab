using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using PlayerSavesKeys = PlayerPrefsSaves.PlayerSavesKeys;

public class ResolutionAndRefreshRateController : MonoBehaviour
{
    private const int defaultRefreshRate = 60;
    private const int minRefreshRate = 5;

    [Header("Настройки графики")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [Header("Настройки экрана")]
    [SerializeField] private Slider refreshRateSlider;
    [SerializeField] private TMP_Text refreshRateLabel;

    private List<Resolution> uniqueResolutions = new List<Resolution>();
    private Dictionary<string, HashSet<RefreshRate>> refreshRatesByResolution = new Dictionary<string, HashSet<RefreshRate>>();
    private RefreshRate selectedRatesByResolution;

    public bool OptionsSaved { get; set; }

    private float RefreshRate
    {
        get => refreshRateSlider.value;
        set
        {
            refreshRateSlider.value = value;
        }
    }

    private void Start()
    {
        if (resolutionDropdown == null || refreshRateSlider == null)
        {
            Debug.LogError("Не все UI-элементы назначены!");
            return;
        }
        
        this.LoadSupportedResolutions();
        this.SetupResolutionDropdown();

        this.SubscribeEvents();

        this.LoadSavedOptions();
    }

    public void SaveChanges()
    {
        var res = uniqueResolutions[resolutionDropdown.value];
        PlayerPrefs.SetInt(PlayerSavesKeys.ScreenWidth, res.width);
        PlayerPrefs.SetInt(PlayerSavesKeys.ScreenHeight, res.height);
        PlayerPrefs.SetInt(PlayerSavesKeys.RefreshRate, (int)this.RefreshRate);
        PlayerPrefs.Save();

        OptionsSaved = true;
    }

    private void SubscribeEvents()
    {
        this.resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        this.refreshRateSlider.onValueChanged.AddListener(OnRefreshRateChanged);
    }

    private void LoadSavedOptions()
    {
        // Загрузка сохранённых настроек
        if (PlayerPrefs.HasKey(PlayerSavesKeys.ScreenWidth))
        {
            int screenWidth = PlayerPrefs.GetInt(PlayerSavesKeys.ScreenWidth);
            int screenHeight = PlayerPrefs.GetInt(PlayerSavesKeys.ScreenHeight);
            int refreshRate = PlayerPrefs.GetInt(PlayerSavesKeys.RefreshRate, defaultRefreshRate);

            int savedResIndex = uniqueResolutions.FindIndex(res => res.width == screenWidth && res.height == screenHeight);
            if (savedResIndex >= 0)
            {
                resolutionDropdown.value = savedResIndex;
                resolutionDropdown.RefreshShownValue();
                OnResolutionChanged(savedResIndex); // Это обновит ползунок
                RefreshRate = refreshRate;
            }
        }
    }

    void LoadSupportedResolutions()
    {
        var allResolutions = Screen.resolutions;
        var resolutionSet = new HashSet<string>();

        foreach (var resolution in allResolutions)
        {
            string key = $"{resolution.width}x{resolution.height}";
            if (!resolutionSet.Contains(key))
            {
                resolutionSet.Add(key);
                uniqueResolutions.Add(resolution);
            }

            // Собираем все герцовки для каждого разрешения
            if (refreshRatesByResolution.ContainsKey(key) == false)
                refreshRatesByResolution[key] = new HashSet<RefreshRate>();

            refreshRatesByResolution[key].Add(resolution.refreshRateRatio);
        }

        // Сортируем разрешения по площади (от большего к меньшему)
        uniqueResolutions = uniqueResolutions
            .OrderByDescending(r => r.width * r.height)
            .ToList();
    }

    void SetupResolutionDropdown()
    {
        OptionsSaved = false;

        var options = new List<TMP_Dropdown.OptionData>();
        foreach (var r in uniqueResolutions)
        {
            string label = $"{r.width} × {r.height}";
            options.Add(new TMP_Dropdown.OptionData(label));
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        // Найти текущее разрешение
        var current = Screen.currentResolution;
        int currentIndex = uniqueResolutions.FindIndex(r =>
            r.width == current.width && r.height == current.height);

        resolutionDropdown.value = Mathf.Max(0, currentIndex);
        resolutionDropdown.RefreshShownValue();

        // Обновить ползунок герцовки под текущее разрешение
        OnResolutionChanged(resolutionDropdown.value);
    }

    void OnResolutionChanged(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= uniqueResolutions.Count)
            return;

        var selectedRes = uniqueResolutions[resolutionIndex];
        selectedRatesByResolution = selectedRes.refreshRateRatio;
        string key = $"{selectedRes.width}x{selectedRes.height}";

        if (refreshRatesByResolution.TryGetValue(key, out var rates) == false)
        {
            Debug.LogWarning($"Нет данных о герцовках для {key}");
            return;
        }

        var sortedRates = rates.OrderBy(x => x).ToList();
        var currentRate = Screen.currentResolution.refreshRateRatio;

        // Если текущая герцовка не в списке — берём максимальную
        if (!sortedRates.Contains(currentRate))
            currentRate = sortedRates.Last();

        // Настраиваем ползунок
        refreshRateSlider.minValue = minRefreshRate;
        refreshRateSlider.maxValue = (float)sortedRates.Last().value;
        refreshRateSlider.wholeNumbers = true;
        RefreshRate = (float)currentRate.value;
    }

    private void OnRefreshRateChanged(float value)
    {
        var rate = (uint)value;
        UpdateRefreshRateLabel(rate);

        var resolution = uniqueResolutions[resolutionDropdown.value];
        var targetRate = new RefreshRate();
        targetRate.denominator = 1;
        targetRate.numerator = rate;

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, targetRate);

        void UpdateRefreshRateLabel(uint rate)
        {
            if (refreshRateLabel != null)
            {
                refreshRateLabel.text = $"{rate} Hz";
            }
        }
    }
}