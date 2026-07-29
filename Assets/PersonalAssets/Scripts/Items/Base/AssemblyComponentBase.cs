namespace ExoLab.StructuralСomponents
{
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Абстракция сборочного компонента (оружия, брони и тд.) с минимальным необходимым набором свойств
    /// </summary>
    public class AssemblyComponentBase : ItemAbstract<AssemblyComponentData>, IAssemblyComponent
    {
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

            GameEvents.AssemblyEvents.RaiseComponentAttached(this);
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
    }
}
