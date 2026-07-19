namespace ExoLab.UI
{
    using UnityEngine;
    using ExoLab.Data;

    /// <summary>
    /// Базовый класс для элементов интерфейса, реагирующих на звуки
    /// при взаимодействии мышкой
    /// </summary>
    public class InteractionElement : InteractionElementAbstract
    {
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip pointerEnterSound;
        [SerializeField] private AudioClip pointerExitSound;
        [SerializeField] private AudioClip pointerMoveSound;

        private AudioSource audioSource => Caches.Instance.Audio.AudioSourceFromCanvas;

        private AudioClip ClickSound 
        {
            get
            {
                if (this.clickSound == null)
                {
                    return Caches.Instance.Audio.ButtonClick;
                }

                return this.clickSound;
            }
        }

        private AudioClip PointerEnterSound
        {
            get
            {
                if (this.pointerEnterSound == null)
                {
                    return Caches.Instance.Audio.ButtonEnter;
                }

                return this.pointerEnterSound;
            }
        }

        private AudioClip PointerExitSound
        {
            get
            {
                if (this.pointerExitSound == null)
                {
                    return Caches.Instance.Audio.ButtonExit;
                }

                return this.pointerExitSound;
            }
        }
        
        private AudioClip PointerMoveSound
        {
            get
            {
                if (this.pointerMoveSound == null)
                {
                    return Caches.Instance.Audio.ButtonMove;
                }

                return this.pointerMoveSound;
            }
        }

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
            this.PlaySoundAfterPointerExit();
        }

        protected override void ActionAfterPointerMove()
        {
            this.PlaySoundAfterPointerMove();
        }

        private void PlaySoundAfterClick()
        {
            if (this.ClickSound == null)
            {
                return;
            }

            this.audioSource.PlayOneShot(this.ClickSound);
        }

        private void PlaySoundAfterPointerEnter()
        {
            if (this.PointerEnterSound == null)
            {
                return;
            }

            this.audioSource.PlayOneShot(this.PointerEnterSound);
        }

        private void PlaySoundAfterPointerExit()
        {
            if (this.PointerExitSound == null)
            {
                return;
            }

            this.audioSource.PlayOneShot(this.PointerExitSound);
        }

        private void PlaySoundAfterPointerMove()
        {
            if (this.PointerMoveSound == null)
            {
                return;
            }

            this.audioSource.PlayOneShot(this.PointerMoveSound);
        }
    }
}
