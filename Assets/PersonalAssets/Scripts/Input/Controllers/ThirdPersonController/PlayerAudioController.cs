namespace ExoLab.Input
{
    using UnityEngine;

    /// <summary>
    /// Handles footstep and landing audio events.
    /// </summary>
    public class PlayerAudioController : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField]
        private AudioClip landingAudioClip;

        [SerializeField]
        private AudioClip[] footstepAudioClips;

        [SerializeField, Range(0, 1)]
        private float footstepAudioVolume = 0.5f;

        private CharacterController controller;

        private void Awake()
        {
            this.controller = GetComponent<CharacterController>();
        }

        public void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (this.footstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, this.footstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(
                        this.footstepAudioClips[index],
                        transform.TransformPoint(this.controller.center),
                        this.footstepAudioVolume);
                }
            }
        }

        public void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && this.landingAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(
                    this.landingAudioClip,
                    transform.TransformPoint(this.controller.center),
                    this.footstepAudioVolume);
            }
        }
    }
}