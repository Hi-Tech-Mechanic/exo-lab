namespace ExoLab.Notifications.Handlers
{
    using System.Collections.Generic;
    using DG.Tweening;
    using ExoLab.Notifications.Views;
    using UnityEngine;

    /// <summary>
    /// Handler for Info notifications.
    /// Displays multiple notifications simultaneously, stacked vertically
    /// from bottom to top on the left side of the screen.
    /// Uses object pooling — no Instantiate/Destroy at runtime.
    ///
    /// Layout:
    ///   [Notification 3] (top, newest)
    ///   [Notification 2]
    ///   [Notification 1] (bottom, oldest)
    /// </summary>
    public sealed class InfoNotificationHandler : BaseNotificationHandler<InfoNotificationView>
    {
        [Header("Info Stacking")]
        [SerializeField] private float notificationSpacing = 10f;
        [SerializeField] private float stackBottomY = 100f;
        [SerializeField] private int maxVisibleNotifications = 10;

        /// <summary>
        /// Tracks currently visible notifications and their assigned Y positions.
        /// Index 0 = bottom (oldest), last = top (newest).
        /// </summary>
        private readonly List<StackedNotification> _stack = new List<StackedNotification>();

        /// <summary>
        /// Info notifications show immediately in parallel, not sequential.
        /// Overrides the base queue-based approach.
        /// </summary>
        public override void Show(NotificationData data)
        {
            // If we reached the max, dismiss the oldest to make room
            while (_stack.Count >= maxVisibleNotifications)
            {
                var oldest = _stack[0];
                oldest.View.ForceKill();
                _stack.RemoveAt(0);
            }

            var view = GetFromPool();
            view.SetNotificationData(data);
            ActiveViews.Add(view);

            // Assign the new notification to the top of the stack
            var stacked = new StackedNotification(view, data);
            _stack.Add(stacked);

            // Recalculate all positions (bottom → top)
            RecalculateStackPositions();

            // Play entrance animation
            var showSeq = view.PlayShowAnimation();

            // Schedule auto-hide after duration
            var holdDuration = Mathf.Max(data.Duration, 1f);
            var fullSequence = DOTween.Sequence();
            fullSequence.Append(showSeq);
            fullSequence.AppendInterval(holdDuration);
            fullSequence.Append(view.PlayHideAnimation());
            fullSequence.OnComplete(() =>
            {
                _stack.Remove(stacked);
                RecalculateStackPositions();
                ActiveViews.Remove(view);
            });
        }

        /// <summary>
        /// Recalculates Y positions for all visible notifications.
        /// Bottom = first in list, Top = last in list.
        /// </summary>
        private void RecalculateStackPositions()
        {
            float currentY = stackBottomY;

            for (int i = 0; i < _stack.Count; i++)
            {
                var stacked = _stack[i];
                var rt = stacked.View.RectTransform;
                var viewHeight = rt.sizeDelta.y;

                // Animate to new position smoothly
                stacked.View.StackY = currentY;
                rt.DOAnchorPosY(currentY, 0.25f)
                    .SetEase(Ease.OutCubic)
                    .SetTarget(rt);

                currentY += viewHeight + notificationSpacing;
            }
        }

        public override void DismissAll()
        {
            foreach (var stacked in _stack.ToArray())
            {
                stacked.View.ForceKill();
            }
            _stack.Clear();
            base.DismissAll();
        }

        /// <summary>
        /// Internal DTO linking a view to its data while it's in the stack.
        /// </summary>
        private readonly struct StackedNotification
        {
            public readonly InfoNotificationView View;
            public readonly NotificationData Data;

            public StackedNotification(InfoNotificationView view, NotificationData data)
            {
                View = view;
                Data = data;
            }
        }
    }
}