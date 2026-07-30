namespace ExoLab.Assembly
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Implementation of <see cref="IItemInspectInputProvider"/> using Unity's new Input System package.
    /// Requires the "Input System" package to be installed via Package Manager.
    /// </summary>
    public sealed class ItemInspectInputProvider : IItemInspectInputProvider
    {
        public Vector2 MousePosition => Mouse.current.position.ReadValue();

        public float MouseX => Mouse.current.delta.ReadValue().x;

        public float MouseY => Mouse.current.delta.ReadValue().y;

        public float MouseScrollWheel => Mouse.current.scroll.ReadValue().y;

        public bool IsLeftMouseButtonPressed => Mouse.current.leftButton.isPressed;

        public bool IsRightMouseButtonPressed => Mouse.current.rightButton.isPressed;

        public bool IsMiddleMouseButtonPressed => Mouse.current.middleButton.isPressed;
    }
}