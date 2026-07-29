using ExoLab.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ExoLab.EditorTools
{
    /// <summary>
    /// Кастомный редактор для CharacteristicLocalization.
    /// Позволяет выбирать тип характеристики из выпадающего списка
    /// всех наследников StatisticAbstract в проекте.
    /// </summary>
    [CustomEditor(typeof(CharacteristicLocalization))]
    public class CharacteristicLocalizationEditor : UnityEditor.Editor
    {
        private CharacteristicLocalization targetScript;

        private SerializedProperty characteristicsProp;

        // Все типы-наследники StatisticAbstract (закешированы)
        private static Type[] cachedTypes;
        private static string[] cachedTypeNames;
        private static string[] cachedTypeFullNames;
        private static bool typesCached;

        private void OnEnable()
        {
            targetScript = target as CharacteristicLocalization;
            characteristicsProp = serializedObject.FindProperty("characteristics");

            CacheStatisticTypes();
        }

        /// <summary>
        /// Найти все типы в сборках, наследующие StatisticAbstract<>
        /// </summary>
        private static void CacheStatisticTypes()
        {
            if (typesCached)
                return;

            var types = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsAbstract || type.IsInterface)
                            continue;

                        if (IsStatisticAbstract(type))
                        {
                            types.Add(type);
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    // Пропускаем сборки, которые не можем загрузить
                }
            }

            cachedTypes = types.ToArray();
            cachedTypeNames = types.Select(t => t.Name).ToArray();
            cachedTypeFullNames = types.Select(t => t.FullName).ToArray();
            typesCached = true;
        }

        private static bool IsStatisticAbstract(Type type)
        {
            Type baseType = type.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType &&
                    baseType.GetGenericTypeDefinition() == typeof(StatisticAbstract<>))
                {
                    return true;
                }
                baseType = baseType.BaseType;
            }
            return false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Characteristic Localization Settings",
                EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            if (GUILayout.Button("Refresh Types"))
            {
                typesCached = false;
                CacheStatisticTypes();
            }

            EditorGUILayout.Space(5);

            // Отображаем список характеристик
            int size = characteristicsProp.arraySize;

            for (int i = 0; i < size; i++)
            {
                SerializedProperty entryProp = characteristicsProp.GetArrayElementAtIndex(i);
                SerializedProperty typeFullNameProp = entryProp.FindPropertyRelative("TypeFullName");
                SerializedProperty translationsProp = entryProp.FindPropertyRelative("Translations");

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.Space(3);

                // Находим текущий индекс типа
                string currentTypeFullName = typeFullNameProp.stringValue;
                int currentIndex = Array.IndexOf(cachedTypeFullNames, currentTypeFullName);

                // Dropdown выбора типа
                int newIndex = EditorGUILayout.Popup(
                    new GUIContent("Characteristic Type"),
                    currentIndex,
                    cachedTypeNames
                );

                if (newIndex != currentIndex && newIndex >= 0)
                {
                    typeFullNameProp.stringValue = cachedTypeFullNames[newIndex];
                }

                EditorGUILayout.Space(5);

                // Отображаем переводы для каждого языка
                EditorGUILayout.LabelField("Translations:", EditorStyles.boldLabel);

                // Гарантируем что размер списка переводов соответствует количеству языков
                EnsureTranslationCount(translationsProp);

                for (int langIndex = 0; langIndex < translationsProp.arraySize; langIndex++)
                {
                    SerializedProperty langEntryProp = translationsProp.GetArrayElementAtIndex(langIndex);
                    SerializedProperty nameProp = langEntryProp.FindPropertyRelative("Name");
                    SerializedProperty unitProp = langEntryProp.FindPropertyRelative("UnitOfMeasurement");

                    string langName = ((Environment.Language)langIndex).ToString();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField($"Language: {langName}", EditorStyles.boldLabel);

                    EditorGUILayout.PropertyField(nameProp, new GUIContent("Name"));
                    EditorGUILayout.PropertyField(unitProp, new GUIContent("Unit Of Measurement"));

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space(2);
                }

                EditorGUILayout.Space(3);

                // Кнопка удаления
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    characteristicsProp.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(5);
            }

            // Кнопка добавления новой характеристики
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Add Characteristic"))
            {
                characteristicsProp.InsertArrayElementAtIndex(size);
                SerializedProperty newEntry = characteristicsProp.GetArrayElementAtIndex(size);
                newEntry.FindPropertyRelative("TypeFullName").stringValue = string.Empty;

                // Создаём пустые записи для каждого языка
                SerializedProperty newTranslations = newEntry.FindPropertyRelative("Translations");
                newTranslations.ClearArray();

                int languageCount = Enum.GetValues(typeof(Environment.Language)).Length;
                for (int langIdx = 0; langIdx < languageCount; langIdx++)
                {
                    newTranslations.InsertArrayElementAtIndex(langIdx);
                    SerializedProperty langEntry = newTranslations.GetArrayElementAtIndex(langIdx);
                    langEntry.FindPropertyRelative("Name").stringValue = string.Empty;
                    langEntry.FindPropertyRelative("UnitOfMeasurement").stringValue = string.Empty;
                }
            }

            EditorGUILayout.Space(10);

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Убедиться, что количество записей переводов соответствует количеству языков
        /// </summary>
        private void EnsureTranslationCount(SerializedProperty translationsProp)
        {
            int languageCount = Enum.GetValues(typeof(Environment.Language)).Length;

            // Удаляем лишние
            while (translationsProp.arraySize > languageCount)
            {
                translationsProp.DeleteArrayElementAtIndex(translationsProp.arraySize - 1);
            }

            // Добавляем недостающие
            while (translationsProp.arraySize < languageCount)
            {
                translationsProp.InsertArrayElementAtIndex(translationsProp.arraySize);
                SerializedProperty langEntry = translationsProp.GetArrayElementAtIndex(translationsProp.arraySize - 1);
                langEntry.FindPropertyRelative("Name").stringValue = string.Empty;
                langEntry.FindPropertyRelative("UnitOfMeasurement").stringValue = string.Empty;
            }
        }
    }
}