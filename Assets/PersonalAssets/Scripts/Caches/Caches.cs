namespace ExoLab.Data
{
    using ExoLab.Constants;
    using System;
    using UnityEngine;

    /// <summary>
    /// Для оптимизации использования данных приложения
    /// </summary>
    public partial class Caches
    {
        /// <summary>
        /// Более безопасное создание экземпляра в многопоточной среде
        /// </summary>
        private static readonly Lazy<Caches> _instance = new Lazy<Caches>(() => new Caches());

        private AudioSource _audioSourceFromCanvas;

        public static Caches Instance => _instance.Value;

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
                    }
                }
                return this._audioSourceFromCanvas;
            }
        }

        /// <summary>
        /// Запрещаем делать экземпляры
        /// </summary>
        private Caches() { }

        //protected virtual void InitHoverSound()
        //{
        //    this.hoverSound = Resources.Load<AudioClip>("Sound/Effects/SFX_Press_Button_Joystick")
        //        ?? throw new System.NullReferenceException();
        //}

        //protected virtual void InitClickSound()
        //{
        //    this.clickSound = Resources.Load<AudioClip>("Sound/Effects/SFX_Press_Button_Keyboard")
        //        ?? throw new System.NullReferenceException();
        //}
    }
}
