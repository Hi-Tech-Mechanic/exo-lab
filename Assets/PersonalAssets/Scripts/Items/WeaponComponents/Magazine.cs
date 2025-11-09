namespace ExoLab.StructuralÑomponents.Weapon
{
    using ExoLab.Data;

    /// <summary>
    /// Ìàãàçèí îðóæèÿ
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

