namespace ExoLab.StructuralСomponents.Suit
{
    using ExoLab.Data;
    using UnityEngine;

    public abstract class SuitComponentAbstract<T> :
        AssemblyComponentBase,
        IDamageable
        where T : SuitComponentItemData
    {
        public new T TypedItemData => (T)base.itemData;

        /// <summary>
        /// Временно - родитель для выбрашенных деталей
        /// </summary>
        private Transform? parentTransform;

        public void GetDamage(double damage, Transform transform)
        {
            //this.Durability -= damage; //todo подумать на счет модели

            if (parentTransform == null)
                parentTransform = transform;

            if (this.TypedItemData.Durability.Value <= 0)
            {
                this.ShootOffPart();
            }
        }

        public void GetDamage(double damage)
        {

        }

        /// <summary>
        /// "Отстрелить" часть
        /// </summary>
        private void ShootOffPart()
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.transform.SetParent(this.parentTransform);
        }
    }
}
