//using Assets.PersonalAssets.Scripts.SuitComponents.ScriptableObjects;
//using System;
//using UnityEngine;

//public class DisplayedItem : AssemblyComponentBase
//{
//    private const string pathToDisplayedItemData = "exo-lab/Item Options/BaseDisplayedItem";
//    private DisplayedItemData DisplayedItemData => Resources.Load<DisplayedItemData>(pathToDisplayedItemData) ??
//        throw new NullReferenceException($"Не найден {nameof(DisplayedItemData)} по пути {pathToDisplayedItemData}");

//    private Sprite iconHolder;

//    protected override void Initialize()
//    {
//        base.Initialize(this.DisplayedItemData);
//        this.iconHolder = this.DisplayedItemData.ItemIcon;
//    }

//    private void Start()
//    {
//        Debug.Log("Icon = " + iconHolder.name);
//        Debug.Log("Icon = " + this.Description);
//        Debug.Log("Icon = " + Weight);
//    }
//}
