namespace ExoLab.UI
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "OptionsUI", menuName = "Scriptable Objects/OptionsUI")]
    public class OptionsUI : ScriptableObject
    {
        public AudioClip? ClickSound;
        public AudioClip? PointerEnterSound;
        public AudioClip? PointerExitSound;
        public AudioClip? PointerMoveSound;
    }
}
