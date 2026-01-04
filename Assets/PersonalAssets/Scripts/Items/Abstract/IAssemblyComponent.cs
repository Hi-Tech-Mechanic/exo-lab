namespace ExoLab.StructuralСomponents
{
    using UnityEngine;

    /// <summary>
    /// Интерфейс компонента, для приведения
    /// </summary>
    public interface IAssemblyComponent
    {
        public void AttachAnObject(GameObject targetObject);
    }
}
