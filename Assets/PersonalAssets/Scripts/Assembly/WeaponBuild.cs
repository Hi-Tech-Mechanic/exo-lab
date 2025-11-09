namespace ExoLab.Assembly
{
    using ExoLab.Constants;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Конструктор оружия
    /// </summary>
    [System.Serializable]
    public class WeaponBuild
    {
        public string weaponId = "ak74";
        public List<string> attachments = new List<string>(); // например: ["wood_stock", "gp25"]

        // Уникальный хэш для кэширования
        public string GetHash()
        {
            string combined = weaponId + "|" + string.Join(",", attachments);
            return combined.GetHashCode().ToString("X8");
        }

        // Для демо: просто возвращаем базовый префаб
        public GameObject GetBasePrefab()
        {
            return Resources.Load<GameObject>($"{Constants.GameResourcesPath.MainFolder}/WeaponComponents/Receivers/СтвольнаяКоробка_1");
        }
    }
}
