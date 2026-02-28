namespace ExoLab
{
    using ExoLab.Data;
    using ExoLab.StructuralСomponents.Suit;
    using System.ComponentModel;
    using UnityEngine;

    public class ExoskeletonAssembly : AssembledStructure
    {
        // todo раскоментить
        protected override void Save()
        {

            //var components = this.transform.GetComponentsInChildren<ArmorPlate>();

            //drillString.components.Add(new AttachedComponent("drill_bit_diamond", "bottom"));

            //throw new System.NotImplementedException();
        }

        // todo раскоментить
        protected override void Load()
        {
            //var loaded = StructurePersistence.Instance.LoadStructure("Exoskeleton_0");
            //if (loaded == null)
            //    return;

            //foreach (var comp in loaded.components)
            //{
            //    var def = ItemRepository.Instance.GetItemById(comp.TypedItemData.Id);
            //    if (def != null && def is AssemblyComponentData assemblyComponent)
            //    {
            //        // Спавните префаб, применяйте иконку, имя и т.д.
            //        Instantiate(assemblyComponent.Prefab, assemblyComponent.AttachmentOptions, false);
            //    }

            //    Debug.Log($"Loaded component: {comp.Name}");
            //    // Здесь можно инстанциировать префабы, применять параметры и т.д.
            //}
        }
    }
}
