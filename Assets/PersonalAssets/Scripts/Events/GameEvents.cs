namespace ExoLab
{
    using ExoLab.StructuralСomponents;
    using System;

    public static partial class GameEvents
    {
        public static event Action<bool> OnAssemblyModeEnabled;

        public static void RaiseAssemblyModeEnabled(bool state)
        {
            OnAssemblyModeEnabled?.Invoke(state);
        }

        public static class Assembly
        {
            public static Action<AssemblyComponentBase> ComponentOnAttached;

            public static void RaiseComponentAttached(AssemblyComponentBase component)
            {
                ComponentOnAttached?.Invoke(component);
            }
        }   
    }
}
