namespace ExoLab.Data
{
    using ExoLab.Constants;
    using System;
    using UnityEngine;

    public partial class Caches
    {
        public class AudioCache
        {
            private AudioSource _audioSourceFromCanvas;

            /// <summary>
            /// Источник звуков из интерфейса
            /// </summary>
            public AudioSource AudioSourceFromCanvas
            {
                get
                {
                    if (this._audioSourceFromCanvas == null)
                    {
                        var gameObject = GameObject.FindWithTag(Constants.Tags.AudioSourceFromCanvas);
                        if (gameObject == null)
                        {
                            throw new NullReferenceException($"Не найден объект с тегом {Constants.Tags.AudioSourceFromCanvas}");
                        }

                        this._audioSourceFromCanvas = gameObject.GetComponent<AudioSource>();
                        if (this._audioSourceFromCanvas == null)
                        {
                            throw new NullReferenceException($"Объект с тегом {Constants.Tags.AudioSourceFromCanvas} не содержит {nameof(AudioSource)}");
                        }
                    }

                    return this._audioSourceFromCanvas;
                }
            }

            public AudioClip ButtonClick
            {
                get
                {
                    return Instance.Interface.OptionsUI?.ClickSound;
                }
            }

            public AudioClip ButtonEnter
            {
                get
                {
                    return Instance.Interface.OptionsUI?.PointerEnterSound;
                }
            }

            public AudioClip ButtonExit
            {
                get
                {
                    return Instance.Interface.OptionsUI?.PointerExitSound;
                }
            }

            public AudioClip ButtonMove
            {
                get
                {
                    return Instance.Interface.OptionsUI?.PointerMoveSound;
                }
            }
        }
    }
}
