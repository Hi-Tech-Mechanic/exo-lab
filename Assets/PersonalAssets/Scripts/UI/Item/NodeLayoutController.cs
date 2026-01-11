namespace ExoLab.UI
{
    using DG.Tweening;
    using ExoLab.Assembly;
    using System;
    using UnityEngine;

    /// <summary>
    /// Контроллер смещений и размеров ноды относительно вращений и зума <see cref="ItemInspect"/>>
    /// </summary>
    [RequireComponent (typeof(NodeInfoPopup))]
    public class NodeLayoutController : MonoBehaviour
    {
        private ItemInspect itemInspector;
        private NodeInfoPopup nodeSelector;

        private Tweener offsetTween;
        private Tweener sizeTween;

        private float duration;
        private Ease easeType;
        private Vector2 rotationOffsetMultiplier;

        private float currentZoomFactor = 1F;
        private float currentRotationDot = 0F;

        private void Awake()
        {
            var itemInspector = GameObject.FindWithTag(Constants.Constants.Tags.ConstructionRoot);
            if (itemInspector == null)
                throw new NullReferenceException($"[{nameof(this.Awake)}] Не найден {Constants.Constants.Tags.ConstructionRoot}");

            this.itemInspector = itemInspector.GetComponent<ItemInspect>(); // this.transform.parent.GetComponentInParent<ItemInspect>();
            this.nodeSelector = this.GetComponent<NodeInfoPopup>();
            this.nodeSelector.InitializeComponents();

            this.duration = this.nodeSelector.NodeOptions.AnimationDuration;
            this.rotationOffsetMultiplier = this.nodeSelector.NodeOptions.rotationOffsetMultiplier;
        }

        private void OnEnable()
        {
            this.itemInspector.OnRotationChanged += this.HandleRotationUpdate;
            this.itemInspector.OnZoomChanged += this.HandleZoomUpdate;
        }

        private void OnDisable()
        {
            this.itemInspector.OnRotationChanged -= this.HandleRotationUpdate;
            this.itemInspector.OnZoomChanged -= this.HandleZoomUpdate;
        }

        private void HandleRotationUpdate(Quaternion rotation)
        {
            this.currentRotationDot = Mathf.Abs(Vector3.Dot(rotation * Vector3.forward, Vector3.right));
            this.UpdateWindowTransform();
        }

        private void HandleZoomUpdate(float currentZ)
        {
            this.currentZoomFactor = this.itemInspector.DefaultCameraDistance / currentZ;
            // Ограничиваем, чтобы окно не стало микроскопическим или огромным
            this.currentZoomFactor = Mathf.Clamp(this.currentZoomFactor, 0.5f, 1f);

            this.UpdateWindowTransform();
        }

        private void UpdateWindowTransform()
        {
            Vector2 rotOffset = Vector2.Lerp(Vector2.one, rotationOffsetMultiplier, currentRotationDot);

            var targetOffset = this.nodeSelector.BaseOffset * rotOffset * this.currentZoomFactor;
            Vector3 targetScale = new Vector3(this.currentZoomFactor, this.currentZoomFactor, this.currentZoomFactor);

            this.offsetTween?.Kill();
            this.sizeTween?.Kill();

            this.offsetTween = DOTween.To(() => this.nodeSelector.CurrentOffset, x => this.nodeSelector.CurrentOffset = x, targetOffset, this.duration)
                .SetEase(easeType);

            this.sizeTween = DOTween.To(() => this.nodeSelector.CurrentScale, x => {
                this.nodeSelector.CurrentScale = x;
            }, targetScale, this.duration).SetEase(this.easeType);
        }
    }
}
