using DG.Tweening;
using ExoLab.Constants;
using UnityEngine;

/// <summary>
/// Навешивать на непосредственно окно которое надо масштабировать
/// </summary>
public class WindowSizeControl : MonoBehaviour
{
    [SerializeField] private bool animationEnabled = true;
    [SerializeField] private float animationDuration = Constants.Timings.Millisecond_200;

    private RectTransform rect;

    private void Awake()
    {
        this.rect = this.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Сменить размер, может быть подвешен на кнопку
    /// </summary>
    public void SetHeight(int height)
    {
        var targetDeltaSize = new Vector2(this.rect.sizeDelta.x, height);

        if (this.animationEnabled)
        {
            this.rect.DOSizeDelta(targetDeltaSize, animationDuration);
        }
        else
        {
            this.rect.sizeDelta = targetDeltaSize;
        }
    }
}
