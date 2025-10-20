using UnityEngine;

public interface IRegeneration : IStatistic
{
    /// <summary>
    /// Регенерация
    /// </summary>
    protected double Regeneration { get; set; }
}
