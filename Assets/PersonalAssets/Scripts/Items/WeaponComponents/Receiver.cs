namespace ExoLab.StructuralСomponents.Weapon
{
    using ExoLab.Assembly;
    using ExoLab.Data;
    using ExoLab.Helpers;
    using UnityEngine;

    /// <summary>
    /// Ствольная коробка
    /// </summary>
    public class Receiver : WeaponComponentAbstract<ReceiverData>
    {
        public override void AttachAnObject(GameObject targetObject)
        {
            var childs = targetObject.GetChilds();
            Transform? constructionRootPoint = null;

            foreach (var child in childs)
            {
                if (child.tag.Equals(Constants.Constants.Tags.PivotPoint))
                {
                    constructionRootPoint = child.transform;
                    break;
                }
            }

            if (constructionRootPoint == null)
            {
                Debug.LogError($"Не была найдена корневая точка конструкции у {targetObject.name}");
                return;
            }

            this.SetAttachmentOptionInCurrentObject(targetObject);

            AssemblyConstruction.OnAttached?.Invoke(this);
        }
    }
}
