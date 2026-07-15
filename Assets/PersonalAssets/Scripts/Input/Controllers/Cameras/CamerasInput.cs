namespace ExoLab.Input
{
    using Assets.PersonalAssets.Scripts.Input.Helpers;
    using System.Collections.Generic;
    using UnityEngine;

    public class CamerasInput : MonoBehaviour, ISubsribable
    {
        public static CamerasInput Instance;

        public Camera? ActiveCamera { get; private set; }

        [SerializeField] private Camera firstPersonCamera;
        [SerializeField] private Camera cameraBack;
        [SerializeField] private Camera cameraForward;

        private void Awake()
        {
            Instance = this;

            this.InitActiveCamera();
        }

        /// <summary>
        /// Set as default first person camera
        /// </summary>  
        private void InitActiveCamera()
        {
            this.ActiveCamera = this.firstPersonCamera;
        }

        private void OnEnable()
        {
            this.SubscribeEvents();
        }

        private void OnDisable()
        {
            this.UnsubscribeEvents();
        }

        public void SubscribeEvents()
        {
            InteractionInputController.OnPressedKeyboard_1 += this.EnableFirstPersonCameraHandler;
            InteractionInputController.OnPressedKeyboard_2 += this.EnableBackCameraHandler;
            InteractionInputController.OnPressedKeyboard_3 += this.EnableForwardCameraHandler;
        }

        public void UnsubscribeEvents()
        {
            InteractionInputController.OnPressedKeyboard_1 += this.EnableFirstPersonCameraHandler;
            InteractionInputController.OnPressedKeyboard_2 += this.EnableBackCameraHandler;
            InteractionInputController.OnPressedKeyboard_3 += this.EnableForwardCameraHandler;
        }

        private void EnableBackCameraHandler()
        {
            this.EnableCamera(this.cameraBack);
        }

        private void EnableForwardCameraHandler()
        {
            this.EnableCamera(this.cameraForward);
        }

        private void EnableFirstPersonCameraHandler()
        {
            this.EnableCamera(this.firstPersonCamera);
        }

        private void EnableCamera(Camera camera)
        {
            List<Camera> allCameras = new()
            {
                this.firstPersonCamera,
                this.cameraBack,
                this.cameraForward
            };

            foreach (var localCamera in allCameras)
            {
                if (localCamera.Equals(camera) == false)
                {
                    localCamera.gameObject.SetActive(false);
                }
            }

            camera.gameObject.SetActive(true);

            this.ActiveCamera = camera;
            CursorStateController.Instance.ToggleCursor(false);
        }
    }
}
