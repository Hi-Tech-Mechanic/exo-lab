using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExoLab.Localization
{
    /// <summary>
    /// Хранит локализованные имена и единицы измерения для характеристик
    /// (StatisticAbstract наследников).
    /// Каждая запись привязана к конкретному типу класса свойства.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacteristicLocalization", menuName = "Localization/Characteristic Localization")]
    public class CharacteristicLocalization : ScriptableObject
    {
        [Serializable]
        public class LanguageEntry
        {
            [Tooltip("Имя характеристики на этом языке")]
            public string Name;

            [Tooltip("Единица измерения на этом языке (например kg., шт., или пусто)")]
            public string UnitOfMeasurement;
        }

        [Serializable]
        public class CharacteristicEntry
        {
            [Tooltip("Тип класса-наследника StatisticAbstract")]
            public string TypeFullName;

            [Tooltip("Перевод для каждого языка")]
            public List<LanguageEntry> Translations;
        }

        [SerializeField]
        [Tooltip("Список характеристик с переводами")]
        private List<CharacteristicEntry> characteristics = new();

        /// <summary>
        /// Получить перевод для указанного типа характеристики и языка
        /// </summary>
        public bool TryGetEntry(Type characteristicType, Environment.Language language,
            out string name, out string unit)
        {
            name = string.Empty;
            unit = string.Empty;

            if (characteristicType == null)
                return false;

            string typeName = characteristicType.FullName;

            var entry = characteristics.FirstOrDefault(e => e.TypeFullName == typeName);
            if (entry == null)
                return false;

            int langIndex = (int)language;
            if (entry.Translations == null || langIndex >= entry.Translations.Count)
                return false;

            var langEntry = entry.Translations[langIndex];
            name = langEntry.Name;
            unit = langEntry.UnitOfMeasurement;
            return true;
        }

        /// <summary>
        /// Получить имя характеристики на указанном языке
        /// </summary>
        public string GetName(Type characteristicType, Environment.Language language)
        {
            if (TryGetEntry(characteristicType, language, out string name, out _))
                return name;

            return characteristicType?.Name ?? "Unknown";
        }

        /// <summary>
        /// Получить единицу измерения на указанном языке
        /// </summary>
        public string GetUnit(Type characteristicType, Environment.Language language)
        {
            if (TryGetEntry(characteristicType, language, out _, out string unit))
                return unit;

            return string.Empty;
        }
    }
}