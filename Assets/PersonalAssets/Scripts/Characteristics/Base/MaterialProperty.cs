using ExoLab;

public class MaterialProperty : StatisticAbstract<string>
{
    public override CharacteristicTypes.Types Type => CharacteristicTypes.Types.Material;

    public override string Value 
    {
        get => base.Value; 
        set
        {   
            if (value == MaterialType.Iron.ToString())
            {
                if (Environment.CurrentLanguage == Environment.Language.RU)
                {
                    base.Value = "Железо";
                    return;
                }

                base.Value = value;
            }
            else if (value == MaterialType.Copper.ToString())
            {
                if (Environment.CurrentLanguage == Environment.Language.RU)
                {
                    base.Value = "Медь";
                    return;
                }

                base.Value = value;
            }
            else if (value == MaterialType.Tin.ToString())
            {
                if (Environment.CurrentLanguage == Environment.Language.RU)
                {
                    base.Value = "Олово";
                    return;
                }

                base.Value = value;
            }
            else if (value == MaterialType.Bronze.ToString())
            {
                if (Environment.CurrentLanguage == Environment.Language.RU)
                {
                    base.Value = "Бронза";
                    return;
                }

                base.Value = value;
            }
            else if (value == MaterialType.Chromium.ToString())
            {
                if (Environment.CurrentLanguage == Environment.Language.RU)
                {
                    base.Value = "Хром";
                    return;
                }

                base.Value = value;
            }
            else if (value == MaterialType.Titanium.ToString())
            {
                if (Environment.CurrentLanguage == Environment.Language.RU)
                {
                    base.Value = "Титан";
                    return;
                }

                base.Value = value;
            }
            else if (value == MaterialType.Plastic.ToString())
            {
                if (Environment.CurrentLanguage == Environment.Language.RU)
                {
                    base.Value = "Пластик";
                    return;
                }

                base.Value = value;
            }
            else if (value == MaterialType.Gold.ToString())
            {
                if (Environment.CurrentLanguage == Environment.Language.RU)
                {
                    base.Value = "Золото";
                    return;
                }

                base.Value = value;
            }
        }
    }

    public enum MaterialType
    {
        Iron,
        Copper,
        Tin,
        Bronze,
        Chromium,
        Titanium,
        Tungsten,
        Plastic,
        Gold
    }
}
