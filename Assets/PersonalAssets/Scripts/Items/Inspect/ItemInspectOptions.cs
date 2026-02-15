namespace ExoLab.Assembly
{
    using UnityEngine;

    /// <summary>
    /// Перечень настроек для <see cref="ItemInspect"/>
    /// </summary>
    [CreateAssetMenu(fileName = "ItemInspect", menuName = "Options/Item inspect options")]
    public class ItemInspectOptions : ScriptableObject
    {
        public float RotationSpeed = 2F;
        public float ZoomSpeed = 5F;
        public float MinCameraDistance = 1F;
        public float MaxCameraDistance = 5F;
        public bool ZoomEnabled = true;
        public bool RotateByCoordinate_X = true;
        public bool RotateByCoordinate_Y = true;
        [Tooltip("Режим поиска сталкиваемых объектов, через Physics.Raycast или GraphicRaycaster")]
        public bool UseGraphicRaycaster = true;

        /// <summary>
        /// Выставляется в <see cref="AssemblyModesController"/>
        /// </summary>
        [Tooltip("Целевой объект который будет вращаться и смешаться")]
        public Transform TargetTransform { get; set; }
    }
}
