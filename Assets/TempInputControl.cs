namespace ExoLab.Input
{
    using DG.Tweening;
    using ExoLab;
    using ExoLab.Constants;
    using StarterAssets;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TempInputControl : MonoBehaviour
    {
        [SerializeField] private Camera _cameraBack;
        [SerializeField] private Camera _cameraForward;

        [SerializeField] private List<Transform> suitComponents;

        [SerializeField] private GameObject assemblyMenu;
        [SerializeField] private GameObject assemblyProps;

        [SerializeField] private GameObject menu;

        private List<Vector3> targetPosition = new();
        private List<Quaternion> targetEulerAngles = new();
        private List<Transform> parentsTransforms = new();

        public GameObject inventory;
        public GameObject stats;

        private delegate void ProcessKey_E();
        private Delegate processKey_E;

        private Camera lastEnabledCamera;

        private bool assemblyMode = false;
        private bool AssemblyMode
        {
            get => assemblyMode;
            set
            {
                lastEnabledCamera = _cameraBack.gameObject.activeInHierarchy ? _cameraBack : _cameraForward;

                if (value == true)
                {
                    this.menu.gameObject.SetActive(false);
                    this.transform.GetChild(0).gameObject.SetActive(false);

                    assemblyMenu.SetActive(true);
                    assemblyProps.SetActive(true);

                    lastEnabledCamera.gameObject.SetActive(false);
                }
                else
                {
                    this.menu.gameObject.SetActive(false);
                    this.transform.GetChild(0).gameObject.SetActive(true);

                    assemblyMenu.SetActive(false);
                    assemblyProps.SetActive(false);

                    lastEnabledCamera.gameObject.SetActive(true);
                }
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

        // По событию изменения настроек клавиш будет обновляться привязки
        private void UpdateKeyBindings(Dictionary<KeyCode, Delegate> bindings)
        {
            foreach (var bind in bindings)
            {
                if (bind.Key.ToString() == Constants.InputButtons.InteractiveButton) //todo
                {
                    this.processKey_E = bind.Value;
                }
            }
        }

        private void AssemblyModeToggle()
        {
            this.AssemblyMode = !this.AssemblyMode;
        }


        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AssemblyMode = false;

                _cameraBack.gameObject.SetActive(true);
                _cameraForward.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (_cameraBack.gameObject.activeInHierarchy == false)
                    return;

                AssemblyMode = false;
                
                _cameraBack.gameObject.SetActive(false);
                _cameraForward.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                inventory.SetActive(!inventory.activeInHierarchy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                stats.SetActive(!stats.activeInHierarchy);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                IEnumerator c = DescroySuit();
                StartCoroutine(c);
                Notifications.InvokeWarnNotify("Разрушение экзоскелета запущено", TransformDirections.RectDirection.Center);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                IEnumerator c = RepairSuit();
                StartCoroutine(c);
                Notifications.InvokeStandardNotify("Регенерация экзоскелета запущена", TransformDirections.RectDirection.TopCenter);
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                this.AssemblyModeToggle();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AssemblyMode = false;
                this.menu.gameObject.SetActive(!menu.activeInHierarchy);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StarterAssetsInputs.Instance.ToggleCursorInputForLook();
                StarterAssetsInputs.Instance.ToggleCursorLocked();
                //DelegateProcess(this.processKey_E);
            }
        }

        IEnumerator DescroySuit()
        {
            foreach (var e in suitComponents)
            {
                //e.GetDamage(100, t);

                e.SetParent(this.transform.parent);
                e.GetComponent<Rigidbody>().isKinematic = false;
                e.GetComponent<Rigidbody>().useGravity = true;

                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator RepairSuit()
        {
            for (int i = 0; i < suitComponents.Count; i++)
            {
                //SuitComponentAbstract<SuitComponentItemData> e = suitComponents[i];
                var e = suitComponents[i];

                e.transform.SetParent(parentsTransforms[i]);
                e.transform.DOLocalMove(targetPosition[i], 0.7f);
                e.transform.DOLocalRotateQuaternion(targetEulerAngles[i], 0.7f);

                e.GetComponent<Rigidbody>().isKinematic = true;
                e.GetComponent<Rigidbody>().useGravity = false;

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
