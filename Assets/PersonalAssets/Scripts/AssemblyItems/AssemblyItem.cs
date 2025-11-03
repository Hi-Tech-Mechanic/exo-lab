using UnityEngine;
using Weapons.Attachments;

public class AssemblyItem : MonoBehaviour
{
    public GameObject child;
    public GameObject parent;

    public void Assembly()
    {
        //child.transform.SetParent(parent.transform);
        //var point = child.GetComponent<MuzzleAttachment>().AttachmentPoint;
        //child.transform.localPosition = point;
    }
}
