namespace ExoLab.Notifications.Views
{
    using DG.Tweening;
    using UnityEngine;

    /// <summary>
    /// Critical notification view.
    /// Overrides the show animation with a camera shake + screen flash effect.
    /// </summary>
    public sealed class CriticalNotificationView : BaseNotificationView
    {
        private const int vibrato = 20;
        private const float randomness = 90F;

        [Header("Critical Settings")]
        [SerializeField, Range(-20F, 20F)]
        private float shakeStrength = 10F;

        [SerializeField, Range(0F, 5F)]
        private float shakeDuration = 0.4F;

        public override Sequence PlayShowAnimation()
        {
            var sequence = base.PlayShowAnimation();

            // Shake the notification rect
            sequence.Join(RectTransform.DOShakePosition(
                shakeDuration,
                shakeStrength,
                vibrato,
                randomness,
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