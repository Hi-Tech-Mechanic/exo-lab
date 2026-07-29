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

            var completedItem = itemObject.AddComponent<CompletedConstructionItem>();
            completedItem.Initialize(model);

            Debug.Log($"[{nameof(ConstructionCompletionService)}] Конструкция '{model.StructureId}' завершена. Компонентов: {model.Components.Count}");
            
            return itemObject;
        }

        /// <summary>
        /// Завершить сборку конструкции и создать предмет, 
        /// прикрепив все компоненты как дочерние объекты.
        /// </summary>
        /// <param name="model">Модель сборной конструкции</param>
        /// <param name="root">Корневой Transform куда будут помещены компоненты</param>
        public static void AssembleInScene(IConstructionModel model, Transform root)
        {
            if (model == null || root == null)
            {
                Debug.LogError($"[{nameof(ConstructionCompletionService)}] Модель или root равны null");
                return;
            }

            foreach (var component in model.Components)
            {
                var prefab = component.TypedItemData.Prefab;
                if (prefab == null)
                {
                    Debug.LogWarning($"[{nameof(ConstructionCompletionService)}] У компонента '{component.ItemData.Name}' нет префаба");
                    continue;
                }

                var instance = Object.Instantiate(prefab, root);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;
            }

            Debug.Log($"[{nameof(ConstructionCompletionService)}] Конструкция '{model.StructureId}' собрана в сцене под '{root.name}'");
        }
    }
}