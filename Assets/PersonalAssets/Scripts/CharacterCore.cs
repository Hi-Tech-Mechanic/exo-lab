using UnityEngine;

namespace SameGame.CharacterSystem
{
    public class CharacterCore : MonoBehaviour
    {
        [SerializeField] private StatsMenu statsMenu;

        //private void OnEnable()
        //{
        //    CharacterSlot.OnPutItem += AddItemStats;
        //    CharacterSlot.OnRemoveItem += RemoveItemStats;
        //}

        //private void OnDisable()
        //{
        //    CharacterSlot.OnPutItem -= AddItemStats;
        //    CharacterSlot.OnRemoveItem -= RemoveItemStats;
        //}

        //private void Start()
        //{
        //    Init();
        //}

        //private void Init()
        //{
        //    CheckSave();
        //}

        //private void CheckSave()
        //{
        //    if (true) // If save is null
        //    {
        //        SetConstStats();
        //    }
        //}

        //private void SetConstStats()
        //{
        //    Health = basicHealth;
        //    Armor = basicArmor;
        //    Damage = basicDamage;
        //    CritChance = basicCritChance;
        //    CritDamage = basicCritDamage;
        //}

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

