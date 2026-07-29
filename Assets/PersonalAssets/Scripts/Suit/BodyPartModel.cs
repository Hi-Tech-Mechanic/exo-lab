namespace Exception
{
    using ExoLab.Data;
    using System;
    using UnityEngine;

    internal class BodyPartModel
    {
        public SuitComponentItemData data;

        public double CurrentHealth { get; private set; }
        public double MaxHealth { get; private set; }
        public bool IsDestroyed => CurrentHealth <= 0;

        /// <summary>
        /// Передается 1.(текущее) и 2.(максимальное) состояние прочности или здоровья
        /// </summary>
        public event Action<double, double> HealthChanged;
        public event Action OnDestroyed;

        public void TakeDamage(double amount)
        {
            if (IsDestroyed) 
                return;

            this.CurrentHealth -= amount;
            Debug.Log(CurrentHealth);
            this.HealthChanged?.Invoke(this.CurrentHealth, this.MaxHealth);

            if (this.IsDestroyed)
            {
                this.OnDestroyed?.Invoke();
            }
        }

        private void Initialize()
        {

        }

        /// <summary>
        /// Использовать <see cref="Initialize"/ в будущем для подгрузки сохранений по событию>
        /// </summary>
        /// <param name="data"></param>
        public BodyPartModel(SuitComponentItemData data)
        {
            this.data = data;
            this.MaxHealth = data.Durability.Value;
            this.CurrentHealth = this.CurrentHealth;
        }
    }
}
