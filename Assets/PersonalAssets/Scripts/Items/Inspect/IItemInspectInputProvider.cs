namespace ExoLab.Assembly
{
    using UnityEngine;

    /// <summary>
    /// Abstraction for input data used by the inspection system.
    /// Enables testability and decouples from Unity's Input API.
    /// </summary>
    public interface IItemInspectInputProvider
    {
        Vector2 MousePosition { get; }
        float MouseX { get; }
        float MouseY { get; }
        float MouseScrollWheel { get; }
        bool IsLeftMouseButtonPressed { get; }
        bool IsRightMouseButtonPressed { get; }
        bool IsMiddleMouseButtonPressed { get; }
    }
}