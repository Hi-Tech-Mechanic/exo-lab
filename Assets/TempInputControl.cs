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
        [SerializeField] private Camera _firstPersonCamera;
        [SerializeField] private Camera _cameraBack;
        [SerializeField] private Camera _cameraForward;

        [SerializeField] private List<Transform> suitComponents;

        [SerializeField] private GameObject assemblyMenu;
        [SerializeField] private GameObject assemblyProps;

        [SerializeField] private GameObject mainMenu;

        private List<Vector3> targetPosition = new();
        private List<Quaternion> targetEulerAngles = new();
        private List<Transform> parentsTransforms = new();

        public GameObject inventory;
        public GameObject stats;

        private delegate void ProcessKey_E();
        private Delegate processKey_E;

        private Camera? lastEnabledCamera;

        private GameObject playerArmature;

        private bool _assemblyMode = false;
        private bool AssemblyMode
        {
            get => _assemblyMode;
            set
            {
                this._assemblyMode = value;

                if (this.lastEnabledCamera == null)
                {
                    this.lastEnabledCamera = _cameraBack.gameObject.activeInHierarchy ? _cameraBack : _cameraForward;
                }

                this.playerArmature.SetActive(!value);
                this.lastEnabledCamera.gameObject.SetActive(!value);

                this.assemblyMenu.SetActive(value);
                this.assemblyProps.SetActive(value);
                
                StarterAssetsInputs.Instance.ToggleCursorInputForLook(!value);
                StarterAssetsInputs.Instance.ToggleCursorLocked(!value);
            }
        }

        private void Awake()
        {
            this.playerArmature = this.transform.GetChild(0).gameObject;

            foreach (var e in suitComponents)
            {
                targetPosition.Add(e.transform.localPosition);
                targetEulerAngles.Add(e.transform.localRotation);
                parentsTransforms.Add(e.transform.parent);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.AssemblyModeToggle(false);

                this._cameraBack.gameObject.SetActive(true);
                this._cameraForward.gameObject.SetActive(false);
                this._firstPersonCamera.gameObject.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (_cameraBack.gameObject.activeInHierarchy == false)
                    return;

                this.AssemblyModeToggle(false);

                this._cameraBack.gameObject.SetActive(false);
                this._cameraForward.gameObject.SetActive(true);
                this._firstPersonCamera.gameObject.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                this.AssemblyModeToggle(false);

                this._cameraBack.gameObject.SetActive(false);
                this._cameraForward.gameObject.SetActive(false);
                this._firstPersonCamera.gameObject.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                inventory.SetActive(!inventory.activeInHierarchy);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                stats.SetActive(!stats.activeInHierarchy);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha5))
            {
                IEnumerator c = DescroySuit();
                StartCoroutine(c);
                Notifications.InvokeWarnNotify("Разрушение экзоскелета запущено", TransformDirections.RectDirection.Center);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha7))
            {
                IEnumerator c = RepairSuit();
                StartCoroutine(c);
                Notifications.InvokeStandardNotify("Регенерация экзоскелета запущена", TransformDirections.RectDirection.TopCenter);
            }
            else if(Input.GetKeyDown(KeyCode.Tab))
            {
                this.AssemblyModeToggle();
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                this.AssemblyModeToggle(false);
                this.ToggleMainMenu();
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

        private void AssemblyModeToggle(bool? state = null)
        {
            if (state != null)
            {
                this.AssemblyMode = (bool)state;
            }
            else
            {
                this.AssemblyMode = !this.AssemblyMode;
            }

            GameEvents.RaiseAssemblyModeEnabled(this.AssemblyMode);
        }

        private void ToggleMainMenu(bool? state = null)
        {
            if (state!= null)
            {
                this.mainMenu.SetActive((bool)state);
                return;
            }

            this.mainMenu.gameObject.SetActive(!this.mainMenu.activeInHierarchy);
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
