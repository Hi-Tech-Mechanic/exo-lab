using Assets.PersonalAssets.ScriptableObjects;
using UnityEngine;

public abstract class SuitComponentAbstract<T> :
    AssemblyComponentAbstract<T>,
    IDamageable
    where T : SuitComponentItemData
{   
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
