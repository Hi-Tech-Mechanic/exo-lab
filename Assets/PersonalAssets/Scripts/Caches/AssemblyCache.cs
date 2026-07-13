namespace ExoLab.Data
{
    using ExoLab.Assembly;
    using System;
    using UnityEngine;
    using ExoLab.Constants;

    public partial class Caches
    {
        public class AssemblyCache
        {
            private Camera? assemblyCamera;
            private ItemInspect? itemInspect;

            /// <summary>
            /// Контроллер просмотрщик объектов
            /// </summary>
            public ItemInspect ItemInspect
            {
                get
                {
                    if (this.itemInspect == null)
                    {
                        var tag = Constants.Tags.ItemInspect;
                        var gameObject = GameObject.FindWithTag(tag);
                        if (gameObject == null)
                        {
                            Debug.LogError($"Не найден объект с тегом {tag}");
                            return null;
                        }

                        // Находим выключенный объект внутри родителя
                        var localItemInspect = gameObject.GetComponentInChildren<ItemInspect>(true);
                        if (localItemInspect == null)
                        {
                            throw new NullReferenceException($"Не найден компонент {nameof(ItemInspect)}");
                        }

                        this.itemInspect = localItemInspect;
                    }

                    return this.itemInspect;
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
        }
    }
}
