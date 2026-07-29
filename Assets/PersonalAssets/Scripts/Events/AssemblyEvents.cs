namespace ExoLab
{
    using ExoLab.StructuralСomponents;
    using System;

    public static partial class GameEvents
    {
        public static class AssemblyEvents
        {
            public static Action<AssemblyComponentBase> ComponentOnAttached;
            public static event Action<bool> OnAssemblyModeEnabled;
            public static event Action OnAssemblyStarted;

            public static void RaiseComponentAttached(AssemblyComponentBase component)
            {
                ComponentOnAttached?.Invoke(component);
            }

            public static void RaiseAssemblyModeEnabled(bool state)
            {
                OnAssemblyModeEnabled?.Invoke(state);
            }

            public static void RaiseOnAssemblyStarted()
            {
                OnAssemblyStarted?.Invoke();
            }
        }
    }
}
