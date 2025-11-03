namespace Weapons.Attachments
{
    using System;
    using Assets.PersonalAssets.ScriptableObjects;

    /// <summary>
    /// Рукоятка
    /// </summary>
    public class Handle : WeaponComponentAbstract<HandleData>
    {
        public int test;

        protected override void InitializeItemData()
        {
            base.InitializeItemData();

            //var requiredItemData = this.ItemData as HandleData;
            //if (requiredItemData == null)
            //    throw new ArgumentException($"{nameof(this.ItemData)} не является необходимым типом: {nameof(HandleData)}");

            test = this.TypedItemData.test;
            Console.Write(test);
        }
    }
}

