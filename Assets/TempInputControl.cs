using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempInputControl : MonoBehaviour
{
    [SerializeField] private Camera _cameraBack;
    [SerializeField] private Camera _cameraForward;

    [SerializeField] private List<SuitComponent> suitComponents;
    [SerializeField] private Transform t;

    public List<Vector3> targetPosition = new();
    public List<Quaternion> targetEulerAngles = new();
    private List<Transform> parentsTransforms = new ();

    public GameObject inventory;
    public GameObject stats;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _cameraBack.gameObject.SetActive(true);    
            _cameraForward.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _cameraBack.gameObject.SetActive(false);
            _cameraForward.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            IEnumerator c = DescroySuit();
            StartCoroutine(c);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            IEnumerator c = RepairSuit();
            StartCoroutine(c);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.SetActive(!inventory.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            stats.SetActive(!stats.activeInHierarchy);
        }
    }

    private void Awake()
    {
        foreach (var e in suitComponents)
        {
            targetPosition.Add(e.transform.localPosition);
            targetEulerAngles.Add(e.transform.localRotation);
            parentsTransforms.Add(e.transform.parent);
        }
    }

    IEnumerator DescroySuit()
    {
        foreach(var e in suitComponents)
        {
            e.GetDamage(100, t);

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator RepairSuit()
    {
        for (int i = 0; i < suitComponents.Count; i++)
        {
            SuitComponent e = suitComponents[i];

            e.transform.DOLocalMove(targetPosition[i], 0.7f);
            e.transform.DOLocalRotateQuaternion(targetEulerAngles[i], 0.7f);

            e.transform.SetParent(parentsTransforms[i]);
            e.GetComponent<Rigidbody>().isKinematic = true;
            e.GetComponent<Rigidbody>().useGravity = false;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
