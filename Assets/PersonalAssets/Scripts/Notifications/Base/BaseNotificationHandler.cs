namespace ExoLab.Notifications
{
    using System.Collections.Generic;
    using DG.Tweening;
    using ExoLab.Data;
    using UnityEngine;

    /// <summary>1
    /// Abstract base handler for a single notification type.
    /// Each handler owns its own object pool and display queue.
    /// Implements the Strategy pattern — concrete handlers define their own
    /// behaviour via virtual method overrides.
    /// </summary>
    /// <typeparam name="TView">The concrete view type this handler manages.</typeparam>
    [RequireComponent(typeof(NotificationController))]
    public abstract class BaseNotificationHandler<TView> : MonoBehaviour
        where TView : BaseNotificationView
    {
        [Header("Pool Settings")]
        [SerializeField] private GameObject notificationPrefab;
        [SerializeField] protected Transform notificationParent;
        [SerializeField] protected int poolInitialSize = 5;

        // Cache the TView component reference.
        private TView _cachedPrefabView;

        // Object pool
        protected readonly Queue<TView> Pool = new Queue<TView>();
        // Currently active views
        protected readonly List<TView> ActiveViews = new List<TView>();

        // Display queue — notifications waiting to be shown
        protected readonly Queue<NotificationData> DisplayQueue = new Queue<NotificationData>();

        // Is an animation currently playing for this handler?
        protected bool IsPlaying;

        protected virtual void Start()
        {
            if (notificationPrefab == null)
            {
                Debug.LogError($"[{GetType().Name}] notificationPrefab is not assigned! Assign a prefab with a {typeof(TView).Name} component.", this);
                return;
            }

            _cachedPrefabView = notificationPrefab.GetComponent<TView>();
            if (_cachedPrefabView == null)
            {
                Debug.LogError($"[{GetType().Name}] The assigned prefab does not have a {typeof(TView).Name} component attached!", this);
                return;
            }

            if (notificationParent == null)
            {
                // Default to HUD canvas children container
                var hud = Caches.Instance.Interface.HudCanvas;
                if (hud != null)
                    notificationParent = hud.transform;
            }

            PrewarmPool();
        }

        /// <summary>
        /// Creates the initial pool of inactive views.
        /// </summary>
        protected virtual void PrewarmPool()
        {
            for (int i = 0; i < poolInitialSize; i++)
            {
                var view = CreateView();
                ReturnToPool(view);
            }
        }

        /// <summary>
        /// Instantiates a new view. Override if you need custom setup.
        /// </summary>
        protected virtual TView CreateView()
        {
            var go = Instantiate(notificationPrefab, notificationParent);
            go.name = $"{notificationPrefab.name}_Pooled";
            var view = go.GetComponent<TView>();
            view.OnReturnToPool = () => ReturnToPool(view);
            return view;
        }

        /// <summary>
        /// Returns a view to the pool (used as callback from the view).
        /// </summary>
        protected virtual void ReturnToPool(TView view)
        {
            if (view == null) return;

            view.gameObject.SetActive(false);
            view.transform.SetParent(notificationParent, false);

            if (!Pool.Contains(view))
                Pool.Enqueue(view);

            ActiveViews.Remove(view);
        }

        /// <summary>
        /// Retrieves an available view from the pool (or creates a new one if empty).
        /// </summary>
        protected virtual TView GetFromPool()
        {
            if (Pool.Count > 0)
                return Pool.Dequeue();

            // Pool exhausted — create a new view (emergency fallback)
            Debug.LogWarning($"[{GetType().Name}] Pool exhausted, creating new instance. Consider increasing poolInitialSize.");
            return CreateView();
        }

        /// <summary>
        /// Public entry point. Queues a notification to be displayed.
        /// </summary>
        public virtual void Show(NotificationData data)
        {
            DisplayQueue.Enqueue(data);
            TryProcessQueue();
        }

        /// <summary>
        /// Tries to display the next notification in the queue.
        /// Concrete handlers can override this to implement interruption logic.
        /// </summary>
        protected virtual void TryProcessQueue()
        {
            if (IsPlaying || DisplayQueue.Count == 0)
                return;

            IsPlaying = true;
            var data = DisplayQueue.Dequeue();
            var view = GetFromPool();

            ActiveViews.Add(view);
            view.SetNotificationData(data);
            var showSequence = view.PlayShowAnimation();

            // Hold for duration, then hide
            var holdDuration = Mathf.Max(data.Duration, 1f);
            var fullSequence = DOTween.Sequence();
            fullSequence.Append(showSequence);
            fullSequence.AppendInterval(holdDuration);
            fullSequence.Append(view.PlayHideAnimation());
            fullSequence.OnComplete(() =>
            {
                IsPlaying = false;
                TryProcessQueue();
            });
        }

        /// <summary>
        /// Forces all active notifications of this type to dismiss immediately.
        /// </summary>
        public virtual void DismissAll()
        {
            foreach (var view in ActiveViews.ToArray())
            {
                view.ForceKill();
            }
            DisplayQueue.Clear();
            IsPlaying = false;
        }

        /// <summary>
        /// Override in critical handler to interrupt current display.
        /// </summary>
        public virtual void InterruptCurrent()
        {
            // Default implementation: do nothing special
        }
    }
}