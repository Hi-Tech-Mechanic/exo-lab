namespace ExoLab.Data
{
    using ExoLab.Assembly;
    using UnityEngine;
    using ExoLab.Constants;

    public partial class Caches
    {
        public class AssemblyCache
        {
            private AssemblyOptions _assemblyOptions;

            /// <summary>
            /// Контроллер просмотрщик объектов
            /// </summary>
            public ItemInspect ItemInspect
            {
                get
                {
                    if (AssemblyCachesForInspector.Instans.ItemInspect == null)
                    {
                        Debug.LogError($"Не назначен объект {nameof(ItemInspect)}");
                        return null;
                    }

                    return AssemblyCachesForInspector.Instans.ItemInspect;
                }
            }

            public AssemblyOptions AssemblyOptions
            {
                get
                {
                    if (this._assemblyOptions == null)
                    {
                        this._assemblyOptions = Resources.Load<AssemblyOptions>($"{Constants.GameResourcesPath.MainFolder}/Assembly/{nameof(AssemblyOptions)}");
                    }

                    return this._assemblyOptions;
                }
            }

            /// <summary>
            /// Камера смотрящая на сборку предметов
            /// </summary>
            public Camera AssemblyCamera
            {
                get
                {
                    if (AssemblyCachesForInspector.Instans.AssemblyCamera == null)
                    {
                        Debug.LogError($"Не назначен объект {nameof(AssemblyCamera)}");
                        return null;
                    }

                    return AssemblyCachesForInspector.Instans.AssemblyCamera;
                }
            }
        }
    }
}
