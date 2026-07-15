namespace ExoLab.Data
{
    using ExoLab.Constants;
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
                            Debug.LogError($"Не найден объект с тегом {Constants.Tags.AudioSourceFromCanvas}");
                            return null;
                        }

                        this._audioSourceFromCanvas = gameObject.GetComponent<AudioSource>();
                        if (this._audioSourceFromCanvas == null)
                        {
                            Debug.LogError($"Объект с тегом {Constants.Tags.AudioSourceFromCanvas} не содержит {nameof(AudioSource)}");
                            return null;
                        }
                    }

                    return this._audioSourceFromCanvas;
                }
            }

            public AudioClip ButtonClick
            {
                get
                {
                    return Instance.Interface.OptionsUI.ClickSound ?? throw new System.NullReferenceException();
                }
            }

            public AudioClip ButtonEnter
            {
                get
                {
                    return Instance.Interface.OptionsUI.PointerEnterSound ?? throw new System.NullReferenceException();
                }
            }

            public AudioClip ButtonExit
            {
                get
                {
                    return Instance.Interface.OptionsUI.PointerExitSound ?? throw new System.NullReferenceException();
                }
            }

            public AudioClip ButtonMove
            {
                get
                {
                    return Instance.Interface.OptionsUI.PointerMoveSound ?? throw new System.NullReferenceException();
                }
            }
        }
    }
}
