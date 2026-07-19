namespace ExoLab
{
    using ExoLab.Data;
    using System;

    public static partial class GameEvents
    {
        public static class UserEvents
        {
            public static event Action OnInventoryToggle;

            public static event Action<ItemData> OnItemCollected;

            public static event Action<ItemData> OnItemRemoved;

            public static event Action<ItemBase> OnItemClicked;
            public static event Action<ItemBase> OnItemHovered;
            public static event Action OnItemMoved;
            public static event Action OnItemUnhovered;

            public static void RaiseItemCollected(ItemData data)
            {
                OnItemCollected?.Invoke(data);
            }

            public static void RaiseItemRemoved(ItemData data)
            {
                OnItemRemoved?.Invoke(data);
            }

            public static void RaiseInventoryToggle()
            {
                OnInventoryToggle?.Invoke();
            }

            public static void RaiseItemClicked(ItemBase item)
            {
                OnItemClicked?.Invoke(item);
            }

            public static void RaiseItemHovered(ItemBase item)
            {
                OnItemHovered?.Invoke(item);
            }

            public static void RaiseItemMoved()
            {
                OnItemMoved?.Invoke();
            }

            public static void RaiseItemUnhovered()
            {
                OnItemUnhovered?.Invoke();
            }
        }
    }
}
