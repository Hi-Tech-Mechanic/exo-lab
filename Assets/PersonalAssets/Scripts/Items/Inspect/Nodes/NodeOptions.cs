namespace ExoLab.Data
{
    using DG.Tweening;
    using ExoLab.Constants;
    using UnityEngine;

    [CreateAssetMenu(fileName = "NodeOptions", menuName = "Scriptable Objects/NodeOptions")]
    public class NodeOptions : ScriptableObject
    {
        [Header("Main Options")]
        public GameObject WindowPrefab;
        public Vector2 BaseOffset = new Vector2(300, 150);
        [Tooltip ("Материал в который окрасится выделенный объект")]
        public Material SelectedStateMaterial;

        [Space(5)]

        [Header("Tween Settings")]
        [Tooltip("Тип замедления в конце")]
        public Ease EaseType = Ease.OutQuint;
        [Tooltip("Длительность анимации")]
        public float AnimationDuration = Constants.Timings.Millisecond_500;

        [Space(5)]

        [Header("Modification Multipliers")]
        [Tooltip("На сколько увеличится смещение при повороте боком")]
        public Vector2 rotationOffsetMultiplier = new Vector2(1.2F, 1.1F);
    }
}
