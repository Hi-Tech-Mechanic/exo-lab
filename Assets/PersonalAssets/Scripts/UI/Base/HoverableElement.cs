namespace ExoLab.UI
{
    using UnityEngine;
    using ExoLab.Data;

    /// <summary>
    /// Один из базовых классов интерфейса, воспроизводит звуки
    /// при срабатывании событий наведения мышкой
    /// </summary>
    public class HoverableElement : HoverableElementAbstract
    {
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip clickSound;

        private AudioSource audioSource => Caches.Instance.AudioSourceFromCanvas;

        protected override void ActionAfterClick()
        {
            this.PlaySoundAfterClick();
        }

        protected override void ActionAfterPointerEnter()
        {
            this.PlaySoundAfterPointerEnter();
        }

        protected override void ActionAfterPointerExit()
        {
            return;
        }

        private void PlaySoundAfterClick()
        {
            if (this.clickSound == null)
                return;

            this.audioSource.PlayOneShot(this.clickSound);
        }

        private void PlaySoundAfterPointerEnter()
        {
            if (this.hoverSound == null)
                return;

            this.audioSource.PlayOneShot(this.hoverSound);
        }     
    }
}
