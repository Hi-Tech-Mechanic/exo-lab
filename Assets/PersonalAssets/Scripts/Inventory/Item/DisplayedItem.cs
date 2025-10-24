using Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects;
using System;
using UnityEngine;

public class DisplayedItem : ComponentBase
{
    private const string pathToDisplayedItemData = "exo-lab/Item Options/BaseDisplayedItem";
    private DisplayedItemData DisplayedItemData => Resources.Load<DisplayedItemData>(pathToDisplayedItemData) ??
        throw new NullReferenceException($"Не найден {nameof(DisplayedItemData)} по пути {pathToDisplayedItemData}");

    private Sprite icon;

    protected override void Initialize()
    {
        base.Initialize(this.DisplayedItemData);
        this.icon = this.DisplayedItemData.ItemIcon;
    }

    private void Start()
    {
        Debug.Log("Icon = " + icon.name);
        Debug.Log("Icon = " + this.Description);
        Debug.Log("Icon = " + Weight);
    }
}
