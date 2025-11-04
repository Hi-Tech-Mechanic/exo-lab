using Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects;
using System;
using UnityEngine;

/// <summary>
/// Класс описывающий абстрактный предмет
/// </summary>
public abstract class ItemAbstract<T> :
    MonoBehaviour,
    IName,
    IDescription,
    IWeight
    where T : ItemData
{
    public virtual string Name { get; protected set; }

    public virtual string Description { get; protected set; }

    public virtual double Weight { get; protected set; }

    [Tooltip("Ссылка на характеристики компонента (ScriptableObject)")]
    public ItemData ItemData;

    /// <summary>
    /// Типизированная информация о компоненте, содержит полную инфомацию о нём
    /// </summary>
    protected T TypedItemData => (T)this.ItemData;

    protected virtual void Awake()
    {
        this.CheckItemData();
        this.InitializeItemData();
    }

    /// <summary>
    /// Инициализация данных предмета из ScriptableObject. Ищет по переданному пути из папки Resources
    /// </summary>
    private void CheckItemData()
    {
        if (ItemData == null)
            throw new NullReferenceException($"Не заданы данные для предмета [{this.gameObject.name}]");

        if (TypedItemData == null)
            throw new NullReferenceException($"Не типизированы данные для предмета [{this.gameObject.name}]");
    }

    /// <summary>
    /// Инициализация полей из полученного ScriptableObject
    /// </summary>
    protected virtual void InitializeItemData()
    {
        this.Name = this.ItemData.Name;
        this.Description = this.ItemData.Description;
        this.Weight = this.ItemData.Weight;
    }
}
