namespace ExoLab
{
    using ExoLab.Data;
    using System;
    using UnityEngine;

    [Serializable]
    public class StoredItem
    {
        public IItemData ItemData;

        [SerializeField]
        private int amount;

        public int Amount
        {
            get => this.amount;
            set
            {
                if (value < 0)
                {
                    this.amount = 0;
                }

                this.amount = value;
            }
        }

        public StoredItem(IItemData itemData, int amount)
        {
            this.ItemData = itemData;
            this.Amount = amount;
        }
    }
}
