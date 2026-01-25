using UnityEngine;
using System.Collections.Generic;
using TMPro;
using static PlayerPrefsSaves;

public class GraphicsQualitySelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown qualityDropdown;

    private void Start()
    {
        if (qualityDropdown == null)
        {
            Debug.LogError("TMP_Dropdown не назначен!");
            return;
        }

        // Заполняем опции, если они не заданы в редакторе
        if (qualityDropdown.options.Count == 0)
        {
            var options = new List<TMP_Dropdown.OptionData>();
            var qualityNames = QualitySettings.names;
            int levelCount = qualityNames.Length;

            for (int i = 0; i < levelCount; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(qualityNames[i]));
            }

            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(options);
        }

        this.SubscribeEvents();

        this.LoadSaves();

        // Устанавливаем текущий уровень качества
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    private void SubscribeEvents()
    {
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }

    private void LoadSaves()
    {
        // Загрузка сохранённых настроек
        if (PlayerPrefs.HasKey(PlayerSavesKeys.GraphicsQuality))
        {
            var qualityIndex = PlayerPrefs.GetInt(PlayerSavesKeys.GraphicsQuality);
            QualitySettings.SetQualityLevel(qualityIndex, true);
        }
    }

    private void OnQualityChanged(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, true);

        PlayerPrefs.SetInt(PlayerSavesKeys.GraphicsQuality, qualityIndex);
        PlayerPrefs.Save();
    }
}