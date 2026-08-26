namespace ExoLab.Assembly.Services
{
    using UnityEngine;
    
    /// <summary>
    /// Сервис для завершения сборки конструкции и сохранения её
    /// в качестве предмета в инвентаре на основе <see cref="ItemBase"/>.
    /// </summary>
    public static class ConstructionCompletionService
    {
        /// <summary>
        /// Завершить сборку конструкции и создать предмет на основе её компонентов.
        /// Создаёт GameObject с <see cref="CompletedConstructionItem"/>,
        /// в котором хранятся все компоненты и итоговые характеристики.
        /// </summary>
        /// <param name="model">Модель сборной конструкции</param>
        /// <param name="parent">Родительский Transform для созданного предмета (опционально)</param>
        /// <returns>GameObject готового предмета с CompletedConstructionItem</returns>
        public static GameObject CompleteConstruction(IConstructionModel model, Transform parent = null)
        {
            if (model == null)
            {
                Debug.LogError($"[{nameof(ConstructionCompletionService)}] Модель конструкции равна null");
                return null;
            }

            var itemObject = new GameObject($"CompletedConstruction_{model.StructureId ?? "Unknown"}");

            if (parent != null)
            {
                itemObject.transform.SetParent(parent);
            }

            itemObject.transform.localPosition = Vector3.zero;
            itemObject.transform.localRotation = Quaternion.identity;
            itemObject.transform.localScale = new Vector3(1, 1, 1);

            var completedItem = itemObject.AddComponent<CompletedConstructionItem>();
            completedItem.Initialize(model);

            SpawnModelInCameraCapturePoint(model, itemObject.transform);

            Debug.Log($"[{nameof(ConstructionCompletionService)}] Конструкция '{model.StructureId}' завершена. Компонентов: {model.Components.Count}");
            
            return itemObject;
        }

        /// <summary>
        /// Assembles the construction in the scene by spawning the prefab
        /// from <see cref="CompletedConstructionItem"/> into the screenshot root point.
        /// </summary>
        /// <param name="model">Construction model</param>
        /// <param name="root">Screenshot root point where the prefab is spawned</param>
        public static void SpawnModelInCameraCapturePoint(IConstructionModel model, Transform root)
        {
            if (model == null || root == null)
            {
                Debug.LogError($"[{nameof(ConstructionCompletionService)}] Model or root are null");
                return;
            }

            var completedItem = root.GetComponentInChildren<CompletedConstructionItem>();
            if (completedItem == null)
            {
                Debug.LogError($"[{nameof(ConstructionCompletionService)}] CompletedConstructionItem not found under '{root.name}'");
                return;
            }

            var prefab = completedItem.ItemData.Prefab;
            if (prefab == null)
            {
                Debug.LogWarning($"[{nameof(ConstructionCompletionService)}] Prefab is not set for '{completedItem.name}'");
                return;
            }

            var instance = Object.Instantiate(prefab, root);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = new Vector3(1, 1, 1);

            Debug.Log($"[{nameof(ConstructionCompletionService)}] Construction '{model.StructureId}' assembled in scene under '{root.name}'");
        }
    }
}