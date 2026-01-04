namespace ExoLab.Data
{
    using ExoLab.Constants;
    using UnityEngine;

    public partial class Caches
    {
        public class AudioCache
        {
            private AudioSource _audioSourceFromCanvas;

            private AudioClip _buttonHover;
            private AudioClip _buttonClick;

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

            public AudioClip ButtonHover
            {
                get
                {
                    if (_buttonHover == null)
                    {
                        _buttonHover = Resources.Load<AudioClip>($"{Constants.GameResourcesPath.MainFolder}/Sound/Effects/SFX_Press_Button_Joystick")
                            ?? throw new System.NullReferenceException();
                    }

                    return _buttonHover;
                }
            }

            public AudioClip ButtonClick
            {
                get
                {
                    if (_buttonClick == null)
                    {
                        _buttonClick = Resources.Load<AudioClip>($"{Constants.GameResourcesPath.MainFolder}/Sound/Effects/SFX_Press_Button_Keyboard")
                            ?? throw new System.NullReferenceException();
                    }

                    return _buttonClick;
                }
            }
        }
    }
}
