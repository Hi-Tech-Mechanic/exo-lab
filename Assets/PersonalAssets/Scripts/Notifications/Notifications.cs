using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ExoLab.Data;
using static ExoLab.Constants.TransformDirections;

/// <summary>
/// Отвечает за уведомления о событиях для пользователя
/// </summary>
public class Notifications : MonoBehaviour
{
    private static Transform notifyParent;
    private static GameObject standardNotifyPrefab;
    private static GameObject warnNotifyPrefab;

    /// <summary>
    /// Обычное всплывающее сообщение
    /// </summary>
    [SerializeField]
    private GameObject standardNotify;

    /// <summary>
    /// Серьезное всплывающее сообщение
    /// </summary>
    [SerializeField]
    private GameObject warnNotify;

    private const float scale = 1.2F;

    private void Awake()
    {
        notifyParent = Caches.Instance.Interface.HudCanvas.transform;
        standardNotifyPrefab = this.standardNotify;
        warnNotifyPrefab = this.warnNotify;
    }

    /// <summary>
    /// Обычное уведомление
    /// </summary>
    /// <param name="message"></param>
    public static void InvokeStandardNotify(string message, RectDirection rectDirection)
    {
        var notify = Instantiate(standardNotifyPrefab, notifyParent);
        var canvasGroup = notify.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            return;

        var text = notify.GetComponentInChildren<TextMeshProUGUI>();
        var rectTransform = notify.GetComponent<RectTransform>();

        SetRectTransformDirection(ref rectTransform, rectDirection);

        text.text = message;
        var notifyHeight = rectTransform.sizeDelta.y;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, notifyHeight);
        rectTransform.localScale = new Vector3(scale, scale, scale);

        var notifyAnimation = HelperAnimation.FadeAndDecreaseSmoothly(canvasGroup, rectTransform);
        notifyAnimation.OnKill(() => Destroy(notify));
    }

    /// <summary>
    /// Уведомление высокого уровня серьезности
    /// </summary>
    public static void InvokeWarnNotify(string message, RectDirection rectDirection, float duration = 5F)
    {
        var notify = Instantiate(warnNotifyPrefab, notifyParent);

        var canvasGroup = notify.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            return;

        var text = notify.GetComponentInChildren<TextMeshProUGUI>();
        var rectTransform = notify.GetComponent<RectTransform>();

        text.text = message;
        rectTransform.localScale = new Vector3(scale, scale, scale);

        SetRectTransformDirection(ref rectTransform, rectDirection);

        var notifyAnimation = HelperAnimation.LoopFade(canvasGroup, 5F);
        notifyAnimation.OnKill(() => Destroy(notify));
    }

    private static void SetRectTransformDirection(ref RectTransform rectTransform, RectDirection rectDirection)
    {
        switch (rectDirection)
        {
            case RectDirection.LeftCenter:
                rectTransform.pivot = new Vector2(0, 0.5F);
                rectTransform.anchorMin = new Vector2(0, 0.5F);
                rectTransform.anchorMax = new Vector2(0, 0.5F);
                break;
            case RectDirection.RightCenter:
                rectTransform.pivot = new Vector2(0.5F, 0);
                rectTransform.anchorMin = new Vector2(0.5F, 0);
                rectTransform.anchorMax = new Vector2(0.5F, 0);
                break;
            case RectDirection.TopCenter:
                rectTransform.pivot = new Vector2(0.5F, 1F);
                rectTransform.anchorMin = new Vector2(0.5f, 1F);
                rectTransform.anchorMax = new Vector2(0.5F, 1F);
                break;
            case RectDirection.BottomCenter:
                rectTransform.pivot = new Vector2(0.5F, 0);
                rectTransform.anchorMin = new Vector2(0.5f, 0);
                rectTransform.anchorMax = new Vector2(0.5F, 0);
                break;
            case RectDirection.Center:
                rectTransform.pivot = new Vector2(0.5F, 0.5F);
                rectTransform.anchorMin = new Vector2(0.5f, 0.5F);
                rectTransform.anchorMax = new Vector2(0.5F, 0.5F);
                break;
        }
    }

    //private static Image? TryGetImage(GameObject @object)
    //{
    //    var canvasGroup = @object.GetComponentInChildren<Image>();

    //    if (canvasGroup == null)
    //    {
    //        canvasGroup = @object.GetComponentInChildren<Image>();
    //    }

    //    return canvasGroup;
    //}
}
