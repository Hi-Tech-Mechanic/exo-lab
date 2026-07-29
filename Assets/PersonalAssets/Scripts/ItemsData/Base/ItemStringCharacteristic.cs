namespace ExoLab.Data
{
    public class ItemCharacteristicTypes
    {
        public struct ItemStringCharacteristic
        {
            public readonly string Name;
            public readonly string Value;

            public ItemStringCharacteristic(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }
        }

        public struct ItemNumericCharacteristic
        {
            public readonly string Name;
            public readonly double Value;

            public ItemNumericCharacteristic(string name, double value)
            {
                this.Name = name;
                this.Value = value;
            }
        }
    }
}
