namespace ExoLab.Notifications.Views
{
    using DG.Tweening;
    using ExoLab.Constants;
    using UnityEngine;

    /// <summary>
    /// Information notification view.
    /// Anchored to the left side, slides in from the left edge.
    /// Multiple info notifications can stack vertically from bottom to top.
    /// </summary>
    public sealed class InfoNotificationView : BaseNotificationView
    {
        [Header("Info Animation")]
        [SerializeField] private float slideInOffsetX = -100f;

        /// <summary>
        /// The Y position assigned by the handler for stacking.
        /// </summary>
        public float StackY { get; set; }

        protected override void Awake()
        {
            base.Awake();

            if (slideInOffsetX >= 0f)
                slideInOffsetX = -100f;
        }

        public override Sequence PlayShowAnimation()
        {
            gameObject.SetActive(true);

            // Reset state
            canvasGroup.alpha = 0f;
            RectTransform.localScale = Vector3.one;

            // Position at the assigned stack Y
            var targetPos = new Vector2(DefaultAnchoredPosition.x, StackY);
            RectTransform.anchoredPosition = targetPos;

            // Start off-screen to the left, then slide in
            var startPos = new Vector2(targetPos.x + slideInOffsetX, targetPos.y);

            var sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(1f, Constants.Timings.Millisecond_300));
            sequence.Join(RectTransform.DOAnchorPos(targetPos, Constants.Timings.Millisecond_400)
                .From(startPos)
                .SetEase(Ease.OutCubic));
            return sequence;
        }

        public override Sequence PlayHideAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(0f, Constants.Timings.Millisecond_200));
            sequence.Join(RectTransform.DOAnchorPosX(
                RectTransform.anchoredPosition.x + slideInOffsetX,
                Constants.Timings.Millisecond_200));
            sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
                OnReturnToPool?.Invoke();
            });
            return sequence;
        }
    }
}