using System;
using UnityEngine;
using static SameGame.StaticFields;

namespace SameGame.CharacterSystem
{
    public class SuitElement : MonoBehaviour
    {
        #region Public Fields

        //[HideInInspector] public float[] StatsArray = new float[StaticFields.StatsCount];
        public static Action<float[], string> OnOpenItemInfoWindow;

        public DropdownList.ItemTags ItemTags
        {
            get { return itemTag; }
            set { itemTag = value; }
        }

        #endregion

        #region Serialized Private Fields

        [SerializeField] private DropdownList.ItemTags itemTag;
        [Space(10)]

        [Header("Item Stats")]
        [Space(5)]
        [SerializeField] private float Health;
        [SerializeField] private float Armor;
        [SerializeField] private float Damage;
        [SerializeField] private float CritChance;
        [SerializeField] private float CritDamage;

        [Header("Other")]
        [Space(5)]
        [SerializeField] private string Comment;
        #endregion

        private void Start()
        {
            InitStatsArray();
        }

        #region Public Methods

        public void OpenInformationWindow()
        {
            //OnOpenItemInfoWindow?.Invoke(StatsArray, Comment);
        }

        #endregion

        #region Private Methods

        private void InitStatsArray()
        {
            //StatsArray[(byte)Stats.Health] = Health;
            //StatsArray[(byte)Stats.Armor] = Armor;
            //StatsArray[(byte)Stats.Damage] = Damage;
            //StatsArray[(byte)Stats.CritChance] = CritChance;
            //StatsArray[(byte)Stats.CritDamage] = CritDamage;
        }

        #endregion
    }
}
