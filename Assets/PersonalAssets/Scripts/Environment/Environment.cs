namespace ExoLab
{
    public class Environment
    {
        public enum Language
        {
            EN = 0,
            RU = 1
        }

        public static Language CurrentLanguage { get; set; } = Language.EN;
    }
}
