namespace ExoLab
{
    using ExoLab.Data;
    using System;

    public static class GameEvents
    {
        public static event Action<ItemData> OnItemCollected;

        public static void RaiseItemCollected(ItemData data)
        {
            OnItemCollected?.Invoke(data);
        }
    }
}
