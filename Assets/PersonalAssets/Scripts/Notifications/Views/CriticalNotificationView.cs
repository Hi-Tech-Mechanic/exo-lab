using DG.Tweening;
using ExoLab.Constants;
using UnityEngine;

namespace ExoLab.Notifications.Views
{
    /// <summary>
    /// Critical notification view.
    /// Overrides the show animation with a camera shake + screen flash effect.
    /// </summary>
    public sealed class CriticalNotificationView : BaseNotificationView
    {
        [Header("Critical Settings")]
        [SerializeField] private float shakeStrength = 10f;
        [SerializeField] private float shakeDuration = 0.4f;

        protected override void Awake()
        {
            base.Awake();

            if (shakeStrength <= 0f)
                shakeStrength = 10f;

            if (shakeDuration <= 0f)
                shakeDuration = 0.4f;
        }

        public override Sequence PlayShowAnimation()
        {
            var sequence = base.PlayShowAnimation();

            // Shake the notification rect
            sequence.Join(RectTransform.DOShakePosition(
                shakeDuration,
                shakeStrength,
                20,
                90f,
                false,
                true));

            // Also pulse the scale for extra urgency
            sequence.Join(RectTransform.DOPunchScale(
                Vector3.one * 0.15f,
                0.3f,
                10,
                0.5f));

            return sequence;
        }
    }
}