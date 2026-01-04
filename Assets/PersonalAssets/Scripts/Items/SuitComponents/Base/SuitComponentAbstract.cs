namespace ExoLab.Structural—omponents.Suit
{
    using ExoLab.Data;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class SuitComponentAbstract<T> :
        AssemblyComponentBase,
        IDamageable
        where T : SuitComponentItemData
    {
        public new T TypedItemData => (T)base.itemData;

        public void GetDamage(double damage, Transform t)
        {
            Durability -= damage;

            if (Durability <= 0)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.transform.SetParent(t);
            }
        }

        public void GetDamage(double damage)
        {

        }
    }
}
