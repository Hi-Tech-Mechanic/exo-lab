namespace ExoLab.UI
{
    using DG.Tweening;
    using ExoLab.Constants;
    using UnityEngine;

    /// <summary>
    /// Кнопка с анимациями
    /// </summary>
    public class CustomButton : HoverableElement
    {
        [SerializeField] private bool animationIsEnabled = true;
        [SerializeField] private float targetScale = Constants.Scales.ScaleMultiplier_110Percent;

        private float startScale;

        protected override void Awake()
        {
            this.startScale = this.gameObject.transform.localScale.x;
        }

        public void InvokeClickAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(this.ActionAfterPointerEnter)
                    .AppendInterval(Constants.Timings.Millisecond_200)
                    .AppendCallback(this.ActionAfterPointerExit);
        }

        protected override void ActionAfterPointerEnter()
        {
            base.ActionAfterPointerEnter();

            if (this.animationIsEnabled)
                this.transform.DOScale(this.targetScale, Constants.Timings.Millisecond_200);
        }

        protected override void ActionAfterPointerExit()
        {
            if (this.animationIsEnabled)
                this.transform.DOScale(this.startScale, Constants.Timings.Millisecond_200);
        }
    }
}
