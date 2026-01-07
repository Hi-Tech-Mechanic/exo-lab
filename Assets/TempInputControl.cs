namespace ExoLab.Input
{
    using DG.Tweening;
    using ExoLab;
    using ExoLab.Constants;
    using ExoLab.Data;
    using ExoLab.StructuralСomponents.Suit;
    using StarterAssets;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TempInputControl : MonoBehaviour
    {
        [SerializeField] private Camera _cameraBack;
        [SerializeField] private Camera _cameraForward;

        [SerializeField] private List<SuitComponentAbstract<SuitComponentItemData>> suitComponents;
        [SerializeField] private Transform t;

        [SerializeField] private GameObject AssemblyParent;

        public List<Vector3> targetPosition = new();
        public List<Quaternion> targetEulerAngles = new();
        private List<Transform> parentsTransforms = new();

        public GameObject inventory;
        public GameObject stats;

        private delegate void ProcessKey_E();
        private Delegate processKey_E;

        private void Awake()
        {
            UpdateKeyBindings();
        }

        // По событию изменения настроек клавиш будет обновляться привязки
        private void UpdateKeyBindings(Dictionary<KeyCode, Delegate> bindings)
        {
            foreach (var bind in bindings)
            {
                if (bind.Key.ToString() == Constants.InputButtons.interactiveButton) //todo
                {
                    this.processKey_E = bind.Value;
                }
            }
        }

        private void UpdateKeyBindings()
        {
            this.processKey_E = InteractiveObject.Instance.keypressDelegate;
        }

        private void DelegateProcess(ProcessKey_E keypressDelegate)
        {
            keypressDelegate(); 
        }

        // Update is called once per frame
        private void Update()
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
                //IEnumerator c = DescroySuit();
                //StartCoroutine(c);
                Notifications.InvokeWarnNotify("Разрушение экзоскелета запущено", TransformDirections.RectDirection.Center);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                //IEnumerator c = RepairSuit();
                //StartCoroutine(c);
                Notifications.InvokeStandardNotify("Регенерация экзоскелета запущена", TransformDirections.RectDirection.TopCenter);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                inventory.SetActive(!inventory.activeInHierarchy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                stats.SetActive(!stats.activeInHierarchy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                AssemblyParent.SetActive(!AssemblyParent.activeInHierarchy);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StarterAssetsInputs.Instance.ToggleCursorInputForLook();
                StarterAssetsInputs.Instance.ToggleCursorLocked();
                //DelegateProcess(this.processKey_E);
            }
        }

        //private void Awake()
        //{
        //    foreach (var e in suitComponents)
        //    {
        //        targetPosition.Add(e.transform.localPosition);
        //        targetEulerAngles.Add(e.transform.localRotation);
        //        parentsTransforms.Add(e.transform.parent);
        //    }
        //}

        IEnumerator DescroySuit()
        {
            foreach (var e in suitComponents)
            {
                e.GetDamage(100, t);

                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator RepairSuit()
        {
            for (int i = 0; i < suitComponents.Count; i++)
            {
                SuitComponentAbstract<SuitComponentItemData> e = suitComponents[i];

                e.transform.DOLocalMove(targetPosition[i], 0.7f);
                e.transform.DOLocalRotateQuaternion(targetEulerAngles[i], 0.7f);

                e.transform.SetParent(parentsTransforms[i]);
                e.GetComponent<Rigidbody>().isKinematic = true;
                e.GetComponent<Rigidbody>().useGravity = false;

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
