namespace ExoLab
{
    using UnityEngine;

    /// <summary>
    /// Объект который можно как то классифицировать
    /// </summary>
    public class ClassificationItem : MonoBehaviour, IName
    {
        public string Name { get; set; } = string.Empty;
    }
}
