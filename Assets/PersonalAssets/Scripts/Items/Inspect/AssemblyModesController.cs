namespace ExoLab.Assembly
{
    using ExoLab.Data;
    using System;
    using UnityEngine;

    /// <summary>
    /// Сменяет режимы сборки
    /// </summary>
    public class AssemblyModesController : MonoBehaviour
    {
        [SerializeField]
        private ItemInspect itemInspect;

        [Header("Настройки для режима сборки оружия")]
        [SerializeField] private ItemInspectOptions weaponPreset;
        [Tooltip("Родительский объект сборной конструкции")]
        [SerializeField] private Transform weaponAssemblyRoot;
        [Space(5)]

        [Header("Настройки для режима сборки экзоскелета")]
        [SerializeField] private ItemInspectOptions exoskeletonPreset;
        [Tooltip("Родительский объект сборной конструкции")]
        [SerializeField] private Transform exoskeletonAssemblyRoot;
        [Space(5)]

        [Header("Настройки для режима сборки ствола скважины")]
        [SerializeField] private ItemInspectOptions wellborePreset;
        [Tooltip("Родительский объект сборной конструкции")]
        [SerializeField] private Transform wellboreAssemblyRoot;

        public static Action<GameObject> OnChangedConstructionRoot;

        private void Awake()
        {
            if (this.itemInspect == null)
            {
                Debug.LogError($"[{nameof(this.itemInspect)}] не назначен");
            }
            if (this.weaponPreset == null)
            {
                Debug.LogError($"[{nameof(this.weaponPreset)}] не назначен");
            }
            if (this.exoskeletonPreset == null)
            {
                Debug.LogError($"[{nameof(this.exoskeletonPreset)}] не назначен");
            }
            if (this.wellborePreset == null)
            {
                Debug.LogError($"[{nameof(this.wellborePreset)}] не назначен");
            }

            this.Init();
        }

        private void Init()
        {
            // Выставляем при первом запуске сборку оружия как стандарт
            this.SetWeaponPreset();
        }

        public void SetWeaponPreset()
        {
            this.weaponPreset.TargetTransform = this.weaponAssemblyRoot;
            this.itemInspect.ItemInspectOptions = this.weaponPreset;
            this.itemInspect.UpdateOptions();

            var newRoot = this.weaponAssemblyRoot.gameObject;
            Caches.Instance.UpdateConstructionRoot(newRoot);
            OnChangedConstructionRoot?.Invoke(newRoot);
        }

        public void SetExoskeletonPreset()
        {
            this.exoskeletonPreset.TargetTransform = this.exoskeletonAssemblyRoot;
            this.itemInspect.ItemInspectOptions = this.exoskeletonPreset;
            this.itemInspect.UpdateOptions();

            var newRoot = this.exoskeletonAssemblyRoot.gameObject;
            Caches.Instance.UpdateConstructionRoot(newRoot);
            OnChangedConstructionRoot?.Invoke(newRoot);
        }

        public void SetWellborePreset()
        {
            this.wellborePreset.TargetTransform = this.wellboreAssemblyRoot;
            this.itemInspect.ItemInspectOptions = this.wellborePreset;
            this.itemInspect.UpdateOptions();

            var newRoot = this.wellboreAssemblyRoot.gameObject;
            Caches.Instance.UpdateConstructionRoot(newRoot);
            OnChangedConstructionRoot?.Invoke(newRoot);
        }
    }
}
