namespace ExoLab.Notifications
{
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using ExoLab.Constants;

    /// <summary>
    /// Abstract base for all notification view prefabs.
    /// Handles common animation lifecycle (show → hold → hide → return-to-pool).
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseNotificationView : MonoBehaviour
    {
        [Header("References (auto-lookup if not assigned)")]
        [SerializeField] protected TextMeshProUGUI titleText;
        [SerializeField] protected TextMeshProUGUI messageText;
        [SerializeField] protected Image iconImage;
        [SerializeField] protected CanvasGroup canvasGroup;

        /// <summary>Called by the pool when the view is returned for reuse.</summary>
        public System.Action OnReturnToPool { get; set; }

        public RectTransform RectTransform { get; private set; }
        protected Vector2 DefaultAnchoredPosition { get; private set; }

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            DefaultAnchoredPosition = RectTransform.anchoredPosition;

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            // Start hidden
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            OnReturnToPool = null;
        }

        public virtual void SetNotificationData(NotificationData data)
        {
            this.messageText.text = data.Message;

            // May not be assigned
            if (this.titleText != null)
            {
                this.titleText.text = data.Title;
            }

            if (this.iconImage != null && data.Icon != null)
            {
                this.iconImage.sprite = data.Icon;
            }

            if (data.Tint.HasValue && iconImage != null)
            {
                this.iconImage.color = data.Tint.Value;
            }
        }

        /// <summary>
        /// Populates the view with data and plays the entrance animation.
        /// Returns a Sequence that the handler can await/kill.
        /// </summary>
        public virtual Sequence PlayShowAnimation()
        {
            gameObject.SetActive(true);

            // Reset state
            canvasGroup.alpha = 0f;
            RectTransform.anchoredPosition = DefaultAnchoredPosition;
            RectTransform.localScale = Vector3.one;

            var sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(1f, Constants.Timings.Millisecond_300));
            sequence.Join(RectTransform.DOAnchorPosY(DefaultAnchoredPosition.y + 20f,
                Constants.Timings.Millisecond_300).From(true).SetEase(Ease.OutBack));
            return sequence;
        }

        /// <summary>
        /// Plays the exit animation and invokes <see cref="OnReturnToPool"/> when finished.
        /// </summary>
        public virtual Sequence PlayHideAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(0f, Constants.Timings.Millisecond_300));
            sequence.Join(RectTransform.DOScale(0.8f, Constants.Timings.Millisecond_300));
            sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
                OnReturnToPool?.Invoke();
            });
            return sequence;
        }

        /// <summary>
        /// Called when the notification is being dismissed prematurely (e.g. by a critical interrupt).
        /// Kill all tweens and return to pool immediately.
        /// </summary>
        public virtual void ForceKill()
        {
            transform.DOKill();
            canvasGroup.DOKill();
            gameObject.SetActive(false);
            OnReturnToPool?.Invoke();
        }
    }
}