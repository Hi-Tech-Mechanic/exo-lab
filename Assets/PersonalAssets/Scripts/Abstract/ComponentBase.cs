using Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects;
using System;
using UnityEngine;

/// <summary>
/// Предмет с минимальным необходимым количеством свойств
/// </summary>
public abstract class ComponentBase :
    MonoBehaviour,
    ISuitComponent,
    IName,
    IDescription,
    IDurability,
    IWeight,
    IMaterial
{
    private const string pathToItemDataBase = "exo-lab/Item Options/BaseItem";
    protected ItemDataBase ItemData => Resources.Load<ItemDataBase>(pathToItemDataBase) ?? 
        throw new NullReferenceException($"Не найден {nameof(ItemDataBase)} по пути {pathToItemDataBase}");

    public virtual string Name { get; protected set; }

    public virtual string Description { get; protected set;  }

    public virtual double Durability { get; protected set; }

    public virtual double Weight { get; protected set; }

    public abstract IMaterial.MaterialType Material { get; }

    protected void Awake()
    {
        this.Initialize();
    }

    protected virtual void Initialize()
    {
        this.Name = this.ItemData.Name;
        this.Description = this.ItemData.Description;
        this.Durability = this.ItemData.Durability;
        this.Weight = this.ItemData.Weight;
    }

    protected virtual void Initialize(ItemDataBase itemData)
    {
        this.Name = itemData.Name;
        this.Description = itemData.Description;
        this.Durability = itemData.Durability;
        this.Weight = itemData.Weight;
    }
}
