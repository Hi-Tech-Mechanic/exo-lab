namespace ExoLab.Assembly.Services
{
    using ExoLab.Helpers;
    using ExoLab.Items;
    using ExoLab.StructuralСomponents;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Представляет завершённую сборную конструкцию как предмет.
    /// Хранит список компонентов и итоговые характеристики.
    /// </summary>
    public class CompletedConstructionItem : ItemBase
    {
        [SerializeField] private List<AssemblyComponentBase> attachedComponents = new();

        /// <summary>
        /// Список прикреплённых компонентов (только для чтения)
        /// </summary>
        public IReadOnlyList<AssemblyComponentBase> AttachedComponents => this.attachedComponents.AsReadOnly();

        /// <summary>
        /// Инициализировать предмет данными из модели конструкции
        /// </summary>
        public void Initialize(IConstructionModel model)
        {
            this.SetObjectName(model);
            this.UpdateAttachedComponents(model.Components);

            this.SetId(model);
            this.SetName(model);
            this.SetDescription(model);
            this.SetMaxStackSize();

            var characteristics = model.GetSumOfAllNumericalCharacteristics();

            this.SetWeight(characteristics);
            this.AddOtherCharacteristics(characteristics);

            this.SetPrefab();
            this.StartCoroutine(this.CaptureIconNextFrame());
        }

        /// <summary>
        /// Add non default properties
        /// </summary>
        private void AddOtherCharacteristics(List<NumericalProperty> characteristics)
        {
            foreach (var characteristic in characteristics)
            {
                if (characteristic.Type is not CharacteristicTypes.Types.Weight &&
                    characteristic.Type is not CharacteristicTypes.Types.MaxStackSize)
                {
                    this.ItemData.Characteristics.Add(characteristic);
                }
            }
        }

        private void UpdateAttachedComponents(IReadOnlyList<AssemblyComponentBase> components)
        {
            this.attachedComponents.Clear();

            foreach (var component in components)
            {
                this.attachedComponents.Add(component);
            }
        }

        private void SetObjectName(IConstructionModel model)
        {
            this.name = $"CompletedConstruction_{model.StructureId ?? "Unknown"}";
        }

        private void SetId(IConstructionModel model)
        {
            this.ItemData.Id = model.StructureId;
        }

        private void SetName(IConstructionModel model)
        {
            this.ItemData.SetName($"Construction: {model.StructureId}");
        }

        private void SetDescription(IConstructionModel model)
        {
            this.ItemData.SetDescription($"Description of the construction: {model.StructureId}");
        }

        private void SetMaxStackSize()
        {
            this.ItemData.SetMaxStackSize(1);
        }

        /// <summary>
        /// Sets the prefab from the first child of the active construction root.
        /// </summary>
        private void SetPrefab()
        {
            var activeRoot = AssemblyConstructionController.Instance?.ActiveConstructionRoot;

            if (activeRoot == null || activeRoot.childCount == 0)
            {
                Debug.LogWarning($"[{nameof(CompletedConstructionItem)}] Active construction root is not available or has no children. Prefab was not set for '{this.name}'");
                return;
            }

            var target = activeRoot.GetChild(0).gameObject;
            this.ItemData.SetPrefab(target);
        }

        private void SetWeight(List<NumericalProperty> characteristics)
        {
            var weight = characteristics.FirstOrNull(x => x.Type == CharacteristicTypes.Types.Weight);
            this.ItemData.SetWeight(weight.Value);
        }

        /// <summary>
        /// Waits one frame for the GPU to upload the prefab meshes,
        /// then captures a screenshot of the assembled construction root as the item icon.
        /// </summary>
        private System.Collections.IEnumerator CaptureIconNextFrame()
        {
            // Wait one frame so the GPU has uploaded the prefab meshes
            yield return null;

            var service = ScreenshotRequestHandler.Instance?.Service;

            if (service == null)
            {
                Debug.LogWarning($"[{nameof(CompletedConstructionItem)}] ScreenshotRequestHandler is not available. Icon was not set for '{this.name}'");
                yield break;
            }

            var root = this.transform.parent;
            if (root == null)
            {
                Debug.LogWarning($"[{nameof(CompletedConstructionItem)}] Construction root is not available. Icon was not set for '{this.name}'");
                yield break;
            }

            var icon = service.CaptureAsSprite(root.gameObject);

            if (icon != null)
            {
                this.ItemData.SetIcon(icon);
            }
            else
            {
                Debug.LogWarning($"[{nameof(CompletedConstructionItem)}] Failed to create screenshot for icon of '{this.name}'");
            }
        }
    }
}