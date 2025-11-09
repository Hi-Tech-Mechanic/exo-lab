namespace ExoLab.UI
{
    using DG.Tweening;
    using ExoLab.Constants;
    using UnityEngine;
    using UnityEngine.UI;
    using System.Linq;

    /// <summary>
    /// Увеличивает вкладку при наведении и уменьшая соседние
    /// </summary>
    public class TabScaler : HoverableElement
    {
        [Header("Параметры размеров")]
        [SerializeField]
        private float hoverWidth;
        [SerializeField]
        private float neighborWidth;

        [Header("Параметры цвета")]
        [SerializeField]
        private Color hoveredColor;

        [Header("Тайминги в секундах")]
        [SerializeField]
        [Range(0F, 10F)]
        private float animationDuration = Constants.Timings.Millisecond_400;

        private LayoutElement myLayout;
        private Image background;

        /// <summary>
        /// Соседние кнопки в строке
        /// </summary>
        private LayoutElement[] otherButtonsInRow;
        private LayoutElement[] allButtonsInRow;

        private float startWidth;
        private Color startColor;

        private void Awake()
        {
            this.myLayout = GetComponent<LayoutElement>();
            this.startWidth = this.myLayout.preferredWidth;
            this.background = this.GetComponent<Image>();
            this.startColor = this.background.color;

            GetButtonsInRow();

            return;

            void GetButtonsInRow()
            {
                this.allButtonsInRow = this.transform.parent.GetComponentsInChildren<LayoutElement>();

                var tempOtherButtons = this.allButtonsInRow.ToList();
                tempOtherButtons.Remove(this.myLayout);
                this.otherButtonsInRow = tempOtherButtons.ToArray();
            }
        }

        protected override void ActionAfterPointerEnter()
        {
            base.ActionAfterPointerEnter();
            this.SelectTab();
        }

        protected override void ActionAfterPointerExit()
        {
            base.ActionAfterPointerExit();
            this.ReturnAllButtonsToStartState();
        }

        private void SelectTab()
        {
            DOTween.To(() => this.myLayout.preferredWidth, x => this.myLayout.preferredWidth = x, this.hoverWidth, this.animationDuration)
                   .SetEase(Ease.OutQuint);

            foreach (var button in this.otherButtonsInRow)
            {
                DOTween.To(() => button.preferredWidth, x => button.preferredWidth = x, neighborWidth, this.animationDuration)
                       .SetEase(Ease.OutQuint);
            }

            if (this.background != null)
            {
                this.background.DOColor(this.hoveredColor, this.animationDuration).SetEase(Ease.OutQuint);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform.parent);

            this.ForceRebuildLayoutImmediate();
        }

        private void ReturnAllButtonsToStartState()
        {
            foreach (var button in this.allButtonsInRow)
            {
                DOTween.To(() => button.preferredWidth, x => button.preferredWidth = x, startWidth, this.animationDuration)
                       .SetEase(Ease.OutQuint);
            }

            if (this.background != null)
            {
                this.background.DOColor(this.startColor, this.animationDuration).SetEase(Ease.OutQuint);
            }

            this.ForceRebuildLayoutImmediate();
        }

        /// <summary>
        /// Важно: заставить Canvas обновить layout СРАЗУ, иначе анимация "зависнет"
        /// </summary>
        private void ForceRebuildLayoutImmediate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform.parent);
        }
    }
}
