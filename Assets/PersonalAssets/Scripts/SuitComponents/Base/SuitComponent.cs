using UnityEngine;

public class SuitComponent : ComponentBase, IDamageable
{   
    private void Update()
    {
        Debug.Log(ItemData.Description);
        Debug.Log(ItemData.Name);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

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
