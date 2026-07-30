namespace ExoLab.Assembly
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Сменяет режимы сборки
    /// </summary>
    public class AssemblyModesController : MonoBehaviour
    {
        /// <summary>
        /// Current preset root
        /// </summary>
        public static Transform ActiveConstructionRoot;
        
        public static Action<ItemInspectOptions> onItemInspectOptions;

        [SerializeField]
        private ItemInspect itemInspect;

        [Header("Настройки режимов сборки")]

        [SerializeField]
        private AssemblyPreset weaponPreset;
        [Space(5)]
        [SerializeField]
        private AssemblyPreset exoskeletonPreset;
        [Space(5)]
        [SerializeField]
        private AssemblyPreset wellborePreset;

        private void Awake()    
        {
            if (this.itemInspect == null)
            {
                Debug.LogError($"[{nameof(this.itemInspect)}] не назначен");
            }
        }

        private void Start()
        {
            this.Init();
        }

        private void Init()
        {
            // Выставляем при первом запуске сборку оружия как стандарт
            this.SetWeaponPreset();
        }

        public void SetWeaponPreset()
        {
            this.weaponPreset.ItemInspectOptions.TargetTransform = this.weaponPreset.RootTransofrm;
            onItemInspectOptions?.Invoke(this.weaponPreset.ItemInspectOptions);

            var newRoot = this.weaponPreset.RootTransofrm;
            ActiveConstructionRoot = newRoot;
        }

        public void SetExoskeletonPreset()
        {
            this.exoskeletonPreset.ItemInspectOptions.TargetTransform = this.exoskeletonPreset.RootTransofrm;
            onItemInspectOptions?.Invoke(this.exoskeletonPreset.ItemInspectOptions);

            var newRoot = this.exoskeletonPreset.RootTransofrm;
            ActiveConstructionRoot = newRoot;
        }

        public void SetWellborePreset()
        {
            this.wellborePreset.ItemInspectOptions.TargetTransform = this.wellborePreset.RootTransofrm;
            onItemInspectOptions?.Invoke(this.wellborePreset.ItemInspectOptions);

            var newRoot = this.wellborePreset.RootTransofrm;
            ActiveConstructionRoot = newRoot;
        }

        [Serializable]
        private struct AssemblyPreset
        {
            [Tooltip("Ссылка на SO-config настроек")]
            public ItemInspectOptions ItemInspectOptions;

            [Tooltip("Родительский объект сборной конструкции")]
            public Transform RootTransofrm;
        }
    }
}
