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
        private static readonly Lazy<Caches> _instance = new Lazy<Caches>(() => new Caches());

        private static readonly Lazy<AudioCache> _audioInstance = new Lazy<AudioCache>(() => new AudioCache());

        private static readonly Lazy<InterfaceCache> _interfaceCache = new Lazy<InterfaceCache>(() => new InterfaceCache());

        private Camera _assemblyCamera;

        /// <summary>
        /// Ссылка на объект <see cref="Caches"/>
        /// </summary>
        public static Caches Instance => _instance.Value;

        public AudioCache Audio => _audioInstance.Value;

        public InterfaceCache Interface => _interfaceCache.Value;

        /// <summary>
        /// Камера смотрящая на сборку предметов
        /// </summary>
        public Camera AssemblyCamera
        {
            get
            {
                if (this._assemblyCamera == null)
                {
                    var gameObject = GameObject.FindWithTag(Constants.Tags.MainCamera);
                    if (gameObject == null)
                    {
                        Debug.LogError($"Не найден объект с тегом {Constants.Tags.MainCamera}");
                        return null;
                    }

                    this._assemblyCamera = gameObject.GetComponent<Camera>();
                    if (this._assemblyCamera == null)
                    {
                        Debug.LogError($"Объект с тегом {Constants.Tags.MainCamera} не содержит {nameof(Canvas)}");
                        return null;
                    }
                }

                return this._assemblyCamera;
            }
        }

        /// <summary>
        /// Запрещаем делать экземпляры
        /// </summary>
        private Caches() { }
    }
}
