using UnityEngine.Events;

namespace ExoLab
{
    public interface IInventoryView
    {
        public void CreateSlots(ushort maxSlotsCount);
        public void FillSlots(StoredItem[] items);
        public void SelectSortMode(int modeIndex);
        public void FillSortDropdown(string[] optionNames, UnityAction<int> valueChangedHandler);
        public void ClearSlots();
    }
}
