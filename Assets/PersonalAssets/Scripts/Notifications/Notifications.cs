using ExoLab.Constants;
using UnityEngine;

/// <summary>
/// Legacy wrapper for backward compatibility.
/// Delegates all calls to the new <see cref="ExoLab.Notifications.NotificationController"/> facade.
/// Will be removed once all callers are migrated.
/// </summary>
[System.Obsolete("Use NotificationController.Instance.ShowInfo/ShowWarning/ShowCritical instead.")]
public static class Notifications
{
    ///// <summary>
    ///// Standard info notification (legacy).
    ///// </summary>
    //public static void InvokeStandardNotify(string message, RectDirection rectDirection)
    //{
    //    var controller = ExoLab.Notifications.NotificationController.Instance;
    //    if (controller != null)
    //        controller.ShowInfo("Уведомление", message);
    //}

    ///// <summary>
    ///// Warning notification (legacy).
    ///// </summary>
    //public static void InvokeWarnNotify(string message, RectDirection rectDirection, float duration = 5F)
    //{
    //    var controller = ExoLab.Notifications.NotificationController.Instance;
    //    if (controller != null)
    //        controller.ShowWarning("Внимание", message, duration);
    //}
}