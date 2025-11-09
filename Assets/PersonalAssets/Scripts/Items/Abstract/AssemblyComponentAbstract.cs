namespace ExoLab.StructuralСomponents
{
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Абстракция сборочного компонента (оружия или брони) с минимальным необходимым набором свойств
    /// </summary>
    public abstract class AssemblyComponentAbstract<T> :
        ItemAbstract<T>
        where T : AssemblyComponentData
    {
        public virtual double Durability { get; protected set; }

        public virtual IMaterial.MaterialType Material { get; protected set; }

        protected Vector3 AttachmentPoint { get; set; }
        protected Vector3 Rotation { get; set; }

        /// <summary>
        /// Присоединить объект
        /// </summary>
        public void AttachAnObject(GameObject parent)
        {
            this.SetTargetAttachProperties(parent);

            this.transform.SetParent(parent.transform);
            this.transform.localPosition = AttachmentPoint;
        }

        protected override void InitializeItemData()
        {
            this.Durability = this.TypedItemData.Durability;
            this.Material = this.TypedItemData.Material;
        }

        /// <summary>
        /// Выставление настроек после сопоставленния принадлежности к родителю
        /// </summary>
        private void SetTargetAttachProperties(GameObject item)
        {
            // TODO проверять есть ли в списке такой тип класса и Name объекта
            var component = item.GetComponent<AssemblyComponentAbstract<T>>();
            if (component == null)
                return;

            var itemData = (WeaponComponentItemData)component.ItemData;
            var thisItemData = (WeaponComponentItemData)this.ItemData;

            foreach (var option in thisItemData.attachmentOptions)
            {
                // Проверка соединяется ли этот объект с переданным
                if (itemData.Name.Equals(option.parentObject.Name))
                {
                    if (ValueIsNotDefault(option.AttachmentPoint))
                        this.AttachmentPoint = option.AttachmentPoint;

                    if (ValueIsNotDefault(option.Rotation))
                        this.Rotation = option.Rotation;
                }
            }

            static bool ValueIsNotDefault(Vector3 value)
            {
                return value != Vector3.zero;
            }
        }
    }
}
