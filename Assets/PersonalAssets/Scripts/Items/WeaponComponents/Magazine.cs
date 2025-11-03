namespace Weapons.Attachments
{
    using Assets.PersonalAssets.ScriptableObjects;

    /// <summary>
    /// Магазин оружия
    /// </summary>
    public class Magazine : WeaponComponentAbstract<MagazineData>, IBullets
    {
        private MagazineData magazineItemData;

        public uint Bullets { get; set; }

        protected override void InitializeItemData()
        {
            base.InitializeItemData();
            this.Bullets = magazineItemData.Bullets;
        }
    }
}

