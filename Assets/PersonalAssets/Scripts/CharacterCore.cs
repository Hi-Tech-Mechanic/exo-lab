using UnityEngine;

namespace SameGame.CharacterSystem
{
    public class CharacterCore : MonoBehaviour
    {
        #region Public Fields
        public float Health
        {
            get { return health;}
            set
            {
                health = value;
                statsMenu.DisplayHealth(health);
            }
        }
        private float health;

        public float Armor
        {
            get { return armor; }
            set
            {
                armor = value;
                statsMenu.DisplayArmor(armor);
            }
        }
        private float armor;

        public float Damage
        {
            get { return damage; }
            set
            {
                damage = value;
                statsMenu.DisplayDamage(damage);
            }
        }
        private float damage;

        public float CritChance
        {
            get { return critChance; }
            set
            {
                critChance = value;
                statsMenu.DisplayCritChance(critChance);
            }
        }
        private float critChance;

        public float CritDamage
        {
            get { return critDamage; }
            set
            {
                critDamage = value;
                statsMenu.DisplayCritDamage(critDamage);
            }
        }
        private float critDamage;
        #endregion

        #region Serialized Fields
        [SerializeField] private StatsMenu statsMenu;
        #endregion

        #region Basic Stats
        private const float basicHealth = 20;
        private const float basicArmor = 2;
        private const float basicDamage = 5;
        private const float basicCritChance = 5;
        private const float basicCritDamage = 200;
        #endregion

        private void OnEnable()
        {
            CharacterSlot.OnPutItem += AddItemStats;
            CharacterSlot.OnRemoveItem += RemoveItemStats;
        }

        private void OnDisable()
        {
            CharacterSlot.OnPutItem -= AddItemStats;
            CharacterSlot.OnRemoveItem -= RemoveItemStats;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            CheckSave();
        }

        private void CheckSave()
        {
            if (true) // If save is null
            {
                SetConstStats();
            }
        }

        private void SetConstStats()
        {
            Health = basicHealth;
            Armor = basicArmor;
            Damage = basicDamage;
            CritChance = basicCritChance;
            CritDamage = basicCritDamage;
        }

        private void AddItemStats(float[] itemStats)
        {
            SetItemStats(itemStats, true);
        }

        private void RemoveItemStats(float[] itemStats)
        {
            SetItemStats(itemStats, false);
        }

        private void SetItemStats(float[] itemStats, bool setPositiveValue)
        {
            int various = 1;

            if (setPositiveValue == false)
                various = -1;

            //if (itemStats[(byte)Stats.Health] != 0)
            //    Health += itemStats[(byte)Stats.Health] * various;
            //if (itemStats[(byte)Stats.Armor] != 0)
            //    Armor += itemStats[(byte)Stats.Armor] * various;
            //if (itemStats[(byte)Stats.Damage] != 0)
            //    Damage += itemStats[(byte)Stats.Damage] * various;
            //if (itemStats[(byte)Stats.CritChance] != 0)
            //    CritChance += itemStats[(byte)Stats.CritChance] * various;
            //if (itemStats[(byte)Stats.CritDamage] != 0)
            //    CritDamage += itemStats[(byte)Stats.CritDamage] * various;
        }
    }
}

