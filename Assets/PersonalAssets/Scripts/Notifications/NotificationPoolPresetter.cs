using UnityEngine;

namespace ExoLab.Notifications
{
    /// <summary>
    /// Optional component that can be placed on a GameObject in the scene
    /// to pre-warm all notification pools at scene load.
    /// Attach this to the same GameObject as NotificationController.
    /// </summary>
    [RequireComponent(typeof(NotificationController))]
    public sealed class NotificationPoolPresetter : MonoBehaviour
    {
        [Header("Pool Sizes (overrides handler defaults)")]
        [SerializeField] private int infoPoolSize = 5;
        [SerializeField] private int warningPoolSize = 3;
        [SerializeField] private int criticalPoolSize = 2;

        private void Awake()
        {
            // The handlers' Start() methods will call PrewarmPool() with their
            // own poolInitialSize values. This component exists to allow
            // overriding those sizes from a single Inspector location.
            //
            // If you need to set pool sizes before Start(), you can call:
            //   var controller = GetComponent<NotificationController>();
            //   // Access handlers via reflection or public properties
            //
            // For now, this is a placeholder for future pre-warm configuration.
            Debug.Log("[NotificationPoolPresetter] Pools will be pre-warmed by handlers on Start().");
        }
    }
}