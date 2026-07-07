namespace ExoLab.Data
{
    using ExoLab.Constants;
    using ExoLab.UI;
    using UnityEngine;

    public partial class Caches
    {
        public class AudioCache
        {
            private AudioSource _audioSourceFromCanvas;
            private OptionsUI _optionsUI;

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
 
            public OptionsUI OptionsUI
            {
                get
                {
                    if (this._optionsUI == null)
                    {
                        this._optionsUI = Resources.Load<OptionsUI>($"{Constants.GameResourcesPath.MainFolder}/UI/{nameof(OptionsUI)}");
                    }

                    return this._optionsUI;
                }
            }

            public AudioClip ButtonClick
            {
                get
                {
                    return this.OptionsUI.ClickSound ?? throw new System.NullReferenceException();
                }
            }

            public AudioClip ButtonEnter
            {
                get
                {
                    return this.OptionsUI.PointerEnterSound ?? throw new System.NullReferenceException();
                }
            }

            public AudioClip ButtonExit
            {
                get
                {
                    return this.OptionsUI.PointerExitSound ?? throw new System.NullReferenceException();
                }
            }

            public AudioClip ButtonMove
            {
                get
                {
                    return this.OptionsUI.PointerMoveSound ?? throw new System.NullReferenceException();
                }
            }
        }
    }
}
