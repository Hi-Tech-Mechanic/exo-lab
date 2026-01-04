using DG.Tweening;
using ExoLab.Constants;
using UnityEngine;

public static class HelperAnimation
{
   public static Sequence FadeAndDecreaseSmoothly(CanvasGroup canvas, RectTransform rectTransform)
    {
        var sequenceFade = DOTween.Sequence();
        sequenceFade.Join(canvas.DOFade(0, Constants.Timings.Millisecond_1000))
                    //.Join(text.DOFade(0, Constants.Timings.Millisecond_1000))
                    .Join(rectTransform.DOScale(0.5F, Constants.Timings.Millisecond_1000));

        var sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOAnchorPosY(0, Constants.Timings.Millisecond_300))
                .Join(rectTransform.DOScale(1F, Constants.Timings.Millisecond_300))
                .AppendInterval(Constants.Timings.Millisecond_2000)
                .Append(sequenceFade)
                .OnComplete(() =>
                {
                    sequence.Kill();
                });

        return sequence;
    }

    public static Sequence LoopFade(CanvasGroup canvasGroup, float duration)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0f, Constants.Timings.Millisecond_400)
                   .SetDelay(0)
                   .SetLoops(-1, LoopType.Yoyo)
                   .SetEase(Ease.InOutQuad)); // Плавное ускорение/замедление


        DOVirtual.DelayedCall(duration, StopFade);

        return sequence;

        void StopFade()
        {
            if (sequence != null)
                sequence.Kill();
        }
    }
}
