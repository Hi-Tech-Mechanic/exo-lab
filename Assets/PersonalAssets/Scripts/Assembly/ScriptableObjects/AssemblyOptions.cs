namespace ExoLab.Assembly
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "AssemblyOptions", menuName = "Scriptable Objects/AssemblyOptions")]
    public class AssemblyOptions : ScriptableObject
    {
        public AudioClip[] ConnectionSound;
    }
}
