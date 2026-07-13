namespace ExoLab.Interaction
{
    using DG.Tweening;
    using ExoLab.Assembly;
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>
    /// Контроллер смещений и размеров ноды относительно вращений и зума <see cref="ItemInspect"/>>
    /// </summary>
    [RequireComponent (typeof(InfoNode))]
    public class InfoNodeController : MonoBehaviour
    {
        private ItemInspect itemInspector;
        private InfoNode infoNode;

        private Tweener offsetTween;
        private Tweener sizeTween;

        private float animationDuration;
        private Ease easeType;
        private Vector2 rotationOffsetMultiplier;

        private float currentZoomFactor = 1F;
        private float currentRotationDot = 0F;

        private void OnEnable()
        {
            GameEvents.OnAssemblyModeEnabled += this.AssemblyModeHandler;
        }

        private void OnDisable()
        {
            GameEvents.OnAssemblyModeEnabled -= this.AssemblyModeHandler;
        }

        private void AssemblyModeHandler(bool assemblyEnabled)
        {
            if (assemblyEnabled)
            {
                this.InitializeComponents();
                this.SubscribeEvents();
            }
            else
            {
                this.UnsubscribeEvents();
            }   
        }

        private void InitializeComponents()
        {
            if (this.itemInspector != null)
                return;

            this.infoNode = this.GetComponent<InfoNode>();
            this.itemInspector = Caches.Instance.Assembly.ItemInspect;
            this.animationDuration = Caches.Instance.Interface.NodeOptions.AnimationDuration;
            this.rotationOffsetMultiplier = Caches.Instance.Interface.NodeOptions.rotationOffsetMultiplier;
        }

        private void SubscribeEvents()
        {
            if (this.itemInspector == null)
                return;

            this.itemInspector.OnRotationChanged += this.HandleRotationUpdate;
            this.itemInspector.OnZoomChanged += this.HandleZoomUpdate;
            this.itemInspector.OnCameraPositionChanged += this.HandleCameraChangeUpdate;
        }

        private void UnsubscribeEvents()
        {
            if (this.itemInspector == null)
                return;

            this.itemInspector.OnRotationChanged -= this.HandleRotationUpdate;
            this.itemInspector.OnZoomChanged -= this.HandleZoomUpdate;
            this.itemInspector.OnCameraPositionChanged -= this.HandleCameraChangeUpdate;
        }

        private void HandleRotationUpdate(Quaternion rotation)
        {
            this.currentRotationDot = Mathf.Abs(Vector3.Dot(rotation * Vector3.forward, Vector3.right));
            this.UpdateWindowTransform();
        }

        private void HandleZoomUpdate(float currentZ)
        {
            this.currentZoomFactor = this.itemInspector.DefaultCameraDistance / currentZ;
            // Ограничиваем, чтобы окно не стало микроскопическим или огромным // todo задать экстремумы
            this.currentZoomFactor = Mathf.Clamp(this.currentZoomFactor, 0.5f, 1f);

            this.UpdateWindowTransform();
        }

        private void HandleCameraChangeUpdate()
        {
            this.UpdateWindowTransform();
        }

        private void UpdateWindowTransform()
        {
            Vector2 rotOffset = Vector2.Lerp(Vector2.one, rotationOffsetMultiplier, currentRotationDot);

            var targetOffset = this.infoNode.BaseOffset * rotOffset * this.currentZoomFactor;
            Vector3 targetScale = new Vector3(this.currentZoomFactor, this.currentZoomFactor, this.currentZoomFactor);

            this.offsetTween?.Kill();
            this.sizeTween?.Kill();

            this.offsetTween = DOTween.To(() => this.infoNode.CurrentOffset, x => this.infoNode.CurrentOffset = x, targetOffset, this.animationDuration)
                .SetEase(easeType);

            this.sizeTween = DOTween.To(() => this.infoNode.CurrentScale, x => {
                this.infoNode.CurrentScale = x;
            }, targetScale, this.animationDuration).SetEase(this.easeType);
        }
    }
}
