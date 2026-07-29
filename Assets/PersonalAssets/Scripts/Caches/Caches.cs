namespace ExoLab.Data
{
    using ExoLab.Constants;
    using System;
    using UnityEngine;

    /// <summary>
    /// Для оптимизации использования данных приложения
    /// </summary>
    public partial class Caches
    {
        /// <summary>
        /// Более безопасное создание экземпляра в многопоточной среде
        /// </summary>
        private static readonly Lazy<Caches> instance = new Lazy<Caches>(() => new Caches());
        private static readonly Lazy<AudioCache> audioInstance = new Lazy<AudioCache>(() => new AudioCache());
        private static readonly Lazy<InterfaceCache> interfaceCache = new Lazy<InterfaceCache>(() => new InterfaceCache());
        private static readonly Lazy<AssemblyCache> assemblyCache = new Lazy<AssemblyCache>(() => new AssemblyCache());
        private static readonly Lazy<ItemsCache> itemsCache = new Lazy<ItemsCache>(() => new ItemsCache());

        public Vector2 ScreenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        
        private Camera? mainCamera;

        /// <summary>
        /// Ссылка на объект <see cref="Caches"/>
        /// </summary>
        public static Caches Instance => instance.Value;

        public AudioCache Audio => audioInstance.Value;
        
        public InterfaceCache Interface => interfaceCache.Value;

        public AssemblyCache Assembly => assemblyCache.Value;

        public ItemsCache Items => itemsCache.Value;

        /// <summary>
        /// Основная камера
        /// </summary>
        public Camera MainCamera
        {
            get
            {
                if (this.mainCamera == null)
                {
                    var tag = Constants.Tags.MainCamera;
                    var gameObject = GameObject.FindWithTag(tag);
                    if (gameObject == null)
                    {
                        Debug.LogError($"Не найден объект с тегом {tag}");
                        return null;
                    }

                    this.mainCamera = gameObject.GetComponent<Camera>();
                    if (this.mainCamera == null)
                    {
                        Debug.LogError($"Объект с тегом {tag} не содержит {nameof(Canvas)}");
                        return null;
                    }
                }

                return this.mainCamera;
            }
        }

        /// <summary>
        /// Запрещаем делать экземпляры
        /// </summary>
        private Caches() { }
    }
}
