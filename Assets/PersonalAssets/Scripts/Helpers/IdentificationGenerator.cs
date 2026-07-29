namespace ExoLab.Helpers
{
    public class IdentificationGenerator
    {
        public static string CreateGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
