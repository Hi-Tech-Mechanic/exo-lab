namespace ExoLab.Data
{
    using ExoLab.Assembly;
    using UnityEngine;

    public class AssemblyCachesForInspector : MonoBehaviour
    {
        private static AssemblyCachesForInspector instans;

        public static AssemblyCachesForInspector Instans => instans;

        public ItemInspect ItemInspect;

        public Camera AssemblyCamera;

        private void Awake()
        {
            instans = this;
        }
    }
}
