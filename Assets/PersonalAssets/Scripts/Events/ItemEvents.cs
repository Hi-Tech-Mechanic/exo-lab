namespace ExoLab
{
    using ExoLab.Data;
    using System;

    public static partial class GameEvents
    {
        public static class Items
        {
            public static event Action<ItemBase> OnBeginDragAction;
            public static event Action<ItemBase> OnDragAction;
            public static event Action<ItemBase> OnEndDragAction;

            public static event Action<ItemData> OnItemCollected;

            public static void RaiseItemCollected(ItemData data)
            {
                OnItemCollected?.Invoke(data);
            }

            public static void RaiseOnBeginDrag(ItemBase item)
            {
                OnBeginDragAction?.Invoke(item);
            }

            public static void RaiseOnDrag(ItemBase item)
            {
                OnDragAction?.Invoke(item);
            }

            public static void RaiseOnEndDrag(ItemBase item)
            {
                OnEndDragAction?.Invoke(item);
            }
        }
    }
}
