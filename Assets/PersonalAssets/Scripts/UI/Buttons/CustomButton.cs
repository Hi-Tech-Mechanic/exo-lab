namespace ExoLab.UI
{
    using DG.Tweening;
    using ExoLab.Constants;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Кнопка с анимациями
    /// </summary>
    public class CustomButton : HoverableElement
    {
        [SerializeField] private bool animationIsEnabled = true;
        [SerializeField] private float targetScale = Constants.Scales.ScaleMultiplier_110Percent;

        private float startScale => this.gameObject.transform.localScale.x;

        /// <summary>
        /// Подсвечиваемая рамка кнопки
        /// </summary>
        private Image buttonEdge;

        private void Awake()
        {
            //var imgChildrens = this.transform.GetComponentsInChildren<Image>(includeInactive: true);
            //foreach(var targetImg in imgChildrens)
            //{
            //    if (targetImg.CompareTag(Constants.Tags.UIComponentEdge))
            //    {
            //        buttonEdge = targetImg;
            //    }
            //}
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
            //if (this.buttonEdge != null)
            //    this.buttonEdge.DOFade(1, Constants.Timings.Millisecond_200);
        }

        protected override void ActionAfterPointerExit()
        {
            if (this.animationIsEnabled)
                this.transform.DOScale(this.startScale, Constants.Timings.Millisecond_200);
            //if (this.buttonEdge != null)
            //    this.buttonEdge.DOFade(0, Constants.Timings.Millisecond_200);
        }
    }
}
