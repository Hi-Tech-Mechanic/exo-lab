namespace ExoLab.Assembly.Base
{
    /// <summary>
    /// Модель экзоскелета
    /// </summary>
    internal class SuitConstructionModel : ConstructionModelBase
    {
        // TODO
        public override void Save()
        {
            //var components = this.transform.GetComponentsInChildren<ArmorPlate>();

            //drillString.components.Add(new AttachedComponent("drill_bit_diamond", "bottom"));

            //throw new System.NotImplementedException();
        }

        // TODO
        public override void Load()
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