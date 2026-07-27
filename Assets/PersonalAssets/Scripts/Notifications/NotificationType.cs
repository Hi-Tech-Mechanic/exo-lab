namespace ExoLab.Notifications
{
    /// <summary>
    /// Defines the severity/type of a notification.
    /// Each type maps to a dedicated Handler with specific behavior.
    /// </summary>
    public enum NotificationType
    {
        Info,
        Warning,
        Critical
    }
}