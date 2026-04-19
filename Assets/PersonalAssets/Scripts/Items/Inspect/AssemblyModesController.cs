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

        [Header("Настройки режимов сборки")]

        [SerializeField]
        private AssemblyPreset weaponPreset;
        [Space(5)]
        [SerializeField]
        private AssemblyPreset exoskeletonPreset;
        [Space(5)]
        [SerializeField]
        private AssemblyPreset wellborePreset;

        public static Action<GameObject> OnChangedConstructionRoot;

        private void Awake()
        {
            if (this.itemInspect == null)
            {
                Debug.LogError($"[{nameof(this.itemInspect)}] не назначен");
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
            this.weaponPreset.ItemInspectOptions.TargetTransform = this.weaponPreset.RootTransofrm;
            this.itemInspect.ItemInspectOptions = this.weaponPreset.ItemInspectOptions;

            this.itemInspect.UpdateOptions();

            var newRoot = this.weaponPreset.RootTransofrm.gameObject;
            Caches.Instance.UpdateConstructionRoot(newRoot);
            OnChangedConstructionRoot?.Invoke(newRoot);
        }

        public void SetExoskeletonPreset()
        {
            this.exoskeletonPreset.ItemInspectOptions.TargetTransform = this.exoskeletonPreset.RootTransofrm;
            this.itemInspect.ItemInspectOptions = this.exoskeletonPreset.ItemInspectOptions;
            this.itemInspect.UpdateOptions();

            var newRoot = this.exoskeletonPreset.RootTransofrm.gameObject;
            Caches.Instance.UpdateConstructionRoot(newRoot);
            OnChangedConstructionRoot?.Invoke(newRoot);
        }

        public void SetWellborePreset()
        {
            this.wellborePreset.ItemInspectOptions.TargetTransform = this.wellborePreset.RootTransofrm;
            this.itemInspect.ItemInspectOptions = this.wellborePreset.ItemInspectOptions;
            this.itemInspect.UpdateOptions();

            var newRoot = this.wellborePreset.RootTransofrm.gameObject;
            Caches.Instance.UpdateConstructionRoot(newRoot);
            OnChangedConstructionRoot?.Invoke(newRoot);
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
