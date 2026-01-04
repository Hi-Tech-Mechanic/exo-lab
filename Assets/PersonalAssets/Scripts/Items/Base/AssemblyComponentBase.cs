namespace ExoLab.StructuralСomponents
{
    using ExoLab.Assembly;
    using ExoLab.Data;
    using ExoLab.Helpers;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Абстракция сборочного компонента (оружия или брони) с минимальным необходимым набором свойств
    /// </summary>
    public class AssemblyComponentBase : ItemAbstract<AssemblyComponentData>,
        IAssemblyComponent,
        IDurability,
        IMaterial
    {
        public virtual double Durability { get; protected set; }

        public virtual IMaterial.MaterialType Material { get; protected set; }

        protected Vector3 AttachmentPoint { get; set; }
        protected Quaternion Rotation { get; set; }

        /// <summary>
        /// Метод для <see cref="AttachmentOptionEditor", облегчает сохранения при верстке конструкций/>
        /// </summary>
        public void SaveAttachmentOptionInGuiEditor()
        {
            var attachmentOption = new AssemblyComponentData.AttachmentOption();

            attachmentOption.ParentData = this.transform.parent.GetComponent<AssemblyComponentBase>().TypedItemData;
            attachmentOption.Rotation = this.transform.localRotation;
            attachmentOption.AttachmentPoint = this.transform.localPosition;

            this.UpdateAttachmentOptions(attachmentOption);

            // Поиск дубликатов, если они есть то обновляем данную настройку, оставляя родителя
            for (int i = 0; i < TypedItemData.AttachmentOptions.Count; i++)
            {
                AssemblyComponentData.AttachmentOption option = this.TypedItemData.AttachmentOptions[i];
                if (string.Equals(option.ParentData.Name, attachmentOption.ParentData.Name))
                {
                    this.TypedItemData.AttachmentOptions[i] = attachmentOption;
                    return;
                }
            }

            this.TypedItemData.AttachmentOptions.Add(attachmentOption);
        }

        /// <summary>
        /// Применить к данному объекту настройки привязок
        /// </summary>
        /// <param name="targetObject">Объект к которому происходит привязка</param>
        public void SetAttachmentOptionInCurrentObject(GameObject targetObject)
        {
            this.transform.SetParent(targetObject.transform);
            this.transform.localPosition = this.AttachmentPoint;
            this.transform.localRotation = this.Rotation;
        }

        /// <summary>
        /// Выставление настроек привязки у текущего объекта
        /// </summary>
        public void UpdateAttachmentOptions(AssemblyComponentData.AttachmentOption option)
        {
            this.AttachmentPoint = option.AttachmentPoint;
            this.Rotation = option.Rotation;
        }

        /// <summary>
        /// Присоединиться к передаваемому объекту
        /// </summary>
        /// <param name="targetObject">Объект к которому происходит привязка</param>
        public void AttachAnObject(GameObject targetObject)
        {
            var component = targetObject.GetComponent<AssemblyComponentBase>();
            if (component == null)
            {
                Debug.LogError($"Не был найден {nameof(AssemblyComponentBase)} у {targetObject.name}");
                return;
            }

            var option = this.TryGetAttachmentOptionAfterCompared(component.TypedItemData);
            if (option == null)
            {
                Debug.LogError($"Не был найден {nameof(AssemblyComponentData.AttachmentOption)} у {targetObject.name}");
                return;
            }

            this.UpdateAttachmentOptions(option);
            this.SetAttachmentOptionInCurrentObject(targetObject);

            AssemblyConstruction.OnAttached?.Invoke(this);
        }

        /// <summary>
        /// Может ли быть объект прикреплен
        /// </summary>
        public bool CanBeAttached(AssemblyComponentData assemblyComponent)
        {
            return this.TryGetAttachmentOptionAfterCompared(assemblyComponent) != null;
        }

        public bool CanBeAttached(GameObject targetObject)
        {
            var component = targetObject.GetComponent<AssemblyComponentBase>();
            if (component == null)
                return false;

            return this.CanBeAttached(component.TypedItemData);
        }

        public override Dictionary<string, object> GetNumericStats()
        {
            var result = new Dictionary<string, object>();

            result.AddRange(base.GetNumericStats());
            result[nameof(this.Durability)] = this.Durability;

            return result;
        }

        public override Dictionary<string, object> GetTranslatedNumericStats()
        {
            var result = new Dictionary<string, object>();

            result.AddRange(base.GetTranslatedNumericStats());
            result["Прочность"] = this.Durability;

            return result;
        }

        protected override void InitializeItemData()
        {
            this.Durability = this.TypedItemData.Durability;
            this.Material = this.TypedItemData.Material;
        }

        /// <summary>
        /// Получить совпавшие настройки привязки после сопоставления с целевым объектом
        /// </summary>
        /// <returns></returns>
        private AssemblyComponentData.AttachmentOption? TryGetAttachmentOptionAfterCompared(AssemblyComponentData targetAssemblyComponent)
        {
            var targetName = targetAssemblyComponent.Name;

            foreach (var option in this.TypedItemData.AttachmentOptions)
            {
                var parentName = option.ParentData.Name;

                // Проверка соединяется ли этот объект с переданным
                if (parentName.Equals(targetName))
                {
                    return option;
                }
            }

            return null;
        }

        //private List<AssemblyComponentData.AttachmentOption> GetAttachmentOptions(IAssemblyComponent assemblyComponent)
        //{
        //    switch (assemblyComponent)
        //    {
        //        case (AssemblyComponentBase<AssemblyComponentData>):
        //            return ((AssemblyComponentBase<AssemblyComponentData>)assemblyComponent).TypedItemData.AttachmentOptions;
        //        case (AssemblyComponentBase<MuzzleAttachmentData>):
        //            return ((AssemblyComponentBase<MuzzleAttachmentData>)assemblyComponent).TypedItemData.AttachmentOptions;
        //    }

        //    return new List<AssemblyComponentData.AttachmentOption>();
        //}
    }
}
