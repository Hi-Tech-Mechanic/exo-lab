namespace ExoLab.StructuralÑomponents.Weapon
{
    using ExoLab.Data;

    /// <summary>
    /// Ìàãàçèí îðóæèÿ
    /// </summary>
    public class Magazine : WeaponComponentAbstract<MagazineData>, IBullets
    {
        private MagazineData magazineItemData;

        private uint? bullets;
        public uint Bullets 
        {
            get
            {
                if (this.bullets != null)
                {
                    return (uint)this.bullets;
                }

                this.bullets = this.magazineItemData.Bullets;
                return (uint)this.bullets;
            }
            set 
            {
                bullets = value;
            }
        }
    }
}

