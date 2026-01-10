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

        private Camera? mainCamera;
        private Camera? assemblyCamera;
        private GameObject? constructionRoot;

        /// <summary>
        /// Ссылка на объект <see cref="Caches"/>
        /// </summary>
        public static Caches Instance => instance.Value;

        public AudioCache Audio => audioInstance.Value;
        public InterfaceCache Interface => interfaceCache.Value;

        /// <summary>
        /// Корневой узел конструкции
        /// </summary>
        public GameObject ConstructionRoot
        {
            get
            {
                if (this.constructionRoot == null)
                {
                    var tag = Constants.Tags.ConstructionRoot;
                    var gameObject = GameObject.FindWithTag(tag);
                    if (gameObject == null)
                    {
                        Debug.LogError($"Не найден объект с тегом {tag}");
                        return null;
                    }

                    this.constructionRoot = gameObject.GetComponent<Transform>().gameObject;
                }

                return this.constructionRoot;
            }
        }

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
        /// Камера смотрящая на сборку предметов
        /// </summary>
        public Camera AssemblyCamera
        {
            get
            {
                if (this.assemblyCamera == null)
                {
                    var tag = Constants.Tags.AssemblyInspectCamera;
                    var gameObject = GameObject.FindWithTag(tag);
                    if (gameObject == null)
                    {
                        Debug.LogError($"Не найден объект с тегом {tag}");
                        return null;
                    }

                    this.assemblyCamera = gameObject.GetComponent<Camera>();
                    if (this.assemblyCamera == null)
                    {
                        Debug.LogError($"Объект с тегом {tag} не содержит {nameof(Canvas)}");
                        return null;
                    }
                }

                return this.assemblyCamera;
            }
        }

        /// <summary>
        /// Запрещаем делать экземпляры
        /// </summary>
        private Caches() { }
    }
}
