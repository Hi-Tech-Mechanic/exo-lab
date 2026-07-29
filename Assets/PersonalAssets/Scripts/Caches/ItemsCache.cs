namespace ExoLab.Data
{
    using UnityEngine;
    using ExoLab.Constants;
    using ExoLab.Localization;

    public partial class Caches
    {
        public class ItemsCache
        {
            private CharacteristicLocalization _characteristicLocalization;

            public CharacteristicLocalization CharacteristicLocalization
            {
                get
                {
                    if (this._characteristicLocalization == null)
                    {
                        this._characteristicLocalization = Resources.Load<CharacteristicLocalization>($"{Constants.GameResourcesPath.MainFolder}/Characteristics/{nameof(CharacteristicLocalization)}");
                    }

                    return this._characteristicLocalization;
                }
            }
         }
    }
}
