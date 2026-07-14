namespace ExoLab.StructuralСomponents
{
    using ExoLab.Data;
    using ExoLab.Helpers;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

    /// <summary>
    /// Абстракция сборочного компонента (оружия, брони и тд.) с минимальным необходимым набором свойств
    /// </summary>
    public class AssemblyComponentBase : ItemAbstract<AssemblyComponentData>,
        IAssemblyComponent,
        IDurability,
        IMaterial
    {
        private double? durability;

        public virtual double Durability 
        {
            get
            {
                if (this.durability != null)
                {
                    return (double)this.durability;
                }

                this.durability = this.TypedItemData.Durability;
                return (double)this.durability;
            }

            protected set
            {
                this.durability  = value;
            }
        }

        public virtual IMaterial.MaterialType Material { get => TypedItemData.Material; /*protected set; */}

        protected Vector3 AttachmentPoint { get; set; }
        protected Quaternion Rotation { get; set; }

        private AudioSource audioSource => Caches.Instance.Audio.AudioSourceFromCanvas;

        /// <summary>
        /// Метод для <see cref="AttachmentOptionEditor", облегчает сохранения при верстке конструкций/>
        /// Сохраняемый объект должен находится внутри своего будущего родителя!
        /// </summary>
        public void SaveAttachmentOptionInGuiEditor()
        {
            var attachmentOption = new AssemblyComponentData.AttachmentOption();

            attachmentOption.ParentData = this.transform.parent.GetComponent<AssemblyComponentBase>().TypedItemData;
            attachmentOption.Rotation = this.transform.localRotation;
            attachmentOption.AttachmentPoint = this.transform.localPosition;
            attachmentOption.Scale = this.transform.localScale;

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
            this.transform.localScale = option.Scale;
        }

        /// <summary>
        /// Присоединиться к передаваемому объекту
        /// </summary>
        /// <param name="targetObject">Объект к которому происходит привязка</param>
        public virtual void AttachAnObject(GameObject targetObject)
        {
            var component = targetObject.GetComponent<AssemblyComponentBase>();
            if (component == null)
            {
                Debug.LogError($"Не был найден {nameof(AssemblyComponentBase)} у {targetObject.name}");
                return;
            }

            AssemblyComponentData.AttachmentOption option = this.TryGetAttachmentOptionAfterCompared(component.TypedItemData);
            if (option == null)
            {
                Debug.LogError($"Не был найден {nameof(AssemblyComponentData.AttachmentOption)} у {targetObject.name}");
                return;
            }

            this.UpdateAttachmentOptions(option);
            this.SetAttachmentOptionInCurrentObject(targetObject);
            this.PlayAssemblySound();

            GameEvents.Assembly.RaiseComponentAttached(this);
        }

        /// <summary>
        /// Может ли быть объект прикреплен
        /// </summary>
        public virtual bool CanBeAttached(AssemblyComponentData assemblyComponent)
        {
            return this.TryGetAttachmentOptionAfterCompared(assemblyComponent) != null;
        }

        public virtual bool CanBeAttached(GameObject targetObject)
        {
            var component = targetObject.GetComponent<AssemblyComponentBase>();
            if (component == null)
                return false;

            return this.CanBeAttached(component.TypedItemData);
        }

        public override Dictionary<string, object> GetAllStats()
        {
            var result = new Dictionary<string, object>();

            result.AddRange(base.GetAllStats());
            result.AddRange(this.GetNumericStats());
            result[nameof(this.Material)] = this.Material;

            foreach (var option in this.TypedItemData.AttachmentOptions)
            {
                result["Parent detail"] = option.ParentData.Name;
            }

            return result;
        }

        public override Dictionary<string, object> GetTranslatedAllStats()
        {
            var result = new Dictionary<string, object>();

            result.AddRange(base.GetTranslatedAllStats());
            result.AddRange(this.GetTranslatedNumericStats());
            result["Материал"] = this.Material;

            foreach (var option in this.TypedItemData.AttachmentOptions)
            {
                result["Родительская деталь"] = option.ParentData.Name;
            }

            return result;
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

        /// <summary>
        /// Получить совпавшие настройки привязки после сопоставления с целевым объектом
        /// </summary>
        /// <returns></returns>
        public AssemblyComponentData.AttachmentOption? TryGetAttachmentOptionAfterCompared(AssemblyComponentData targetAssemblyComponent)
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

        private void PlayAssemblySound()
        {
            var sounds = Caches.Instance.Assembly.AssemblyOptions.ConnectionSound;
            var soundNumber = Random.Range(0, sounds.Length);
            var sound = sounds[soundNumber];

            this.audioSource.PlayOneShot(sound);
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
