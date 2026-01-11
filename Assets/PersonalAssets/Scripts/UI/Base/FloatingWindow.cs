using Assets.PersonalAssets.Scripts.UI.Base;
using DG.Tweening;
using ExoLab.Constants;
using ExoLab.Data;
using ExoLab.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ѕлавающее окно которое можно перемещать,
/// €вл€етс€ оберткой дл€ другого окна или панели
/// </summary>
public class FloatingWindow : DraggableElementAbstract, IPointerClickHandler
{
    private const float openWindowDuration = Constants.Timings.Millisecond_400;

    [SerializeField] private Transform contentHolder;

    private Canvas canvas;

    private RectTransform windowRectTransform;
    private Vector2 startWindowDeltaSize;
    private Vector2 windowDeltaSizeAfterChange;

    private bool isHide = false;

    public string WindowName { get; set; }

    private void Awake()
    {
        this.canvas = GetComponentInParent<Canvas>();
     
        this.windowRectTransform = this.transform as RectTransform;
        this.startWindowDeltaSize = this.windowRectTransform.sizeDelta;
    }

    /// <summary>
    /// ¬ставить контент в обертку данного окна
    /// </summary>
    /// <param name="panel">“о что вставл€етс€ в окно</param>
    public void InitializeWindow(GameObject panel, string windowName)
    {
        var panelObject = Instantiate(panel, this.contentHolder);
        var panelRect = panelObject.GetComponent<RectTransform>();

        var header = this.gameObject.TryGetChildWithName("Name");
        var headerText = header.GetComponent<TextMeshProUGUI>();
        this.WindowName = windowName;
        headerText.text = windowName;

        panelRect.localPosition = Caches.Instance.ScreenCenter;
        var height = panelRect.sizeDelta.y + this.startWindowDeltaSize.y;
        var width = panelRect.sizeDelta.x;
        var newDeltaSize = new Vector2(0, height);
        this.windowRectTransform.sizeDelta = newDeltaSize;

        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.anchoredPosition = new Vector2(0, 0);
        panelRect.sizeDelta = new Vector2(0, 0);

        var targetDeltaSize = new Vector2(width, this.windowRectTransform.sizeDelta.y);
        this.windowRectTransform.DOSizeDelta(targetDeltaSize, openWindowDuration).SetEase(Ease.OutQuad);
        this.windowDeltaSizeAfterChange = targetDeltaSize;
    }

    /// <summary>
    /// ѕереключатель скрыт/не скрыт
    /// </summary>
    public void HideToggle()
    {
        this.isHide = !this.isHide;

        if (this.isHide)
        {
            this.AnimatedHide();
        }
        else
        {
            this.AnimatedUnhide();
        }
    }

    public void CloseWindow()
    {
        FloatingWindowsController.Instance.DeleteWindow(this.gameObject);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (this.canvas != null)
        {
            // ѕреобразуем смещение в пространство Canvas
            var success = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvas.transform as RectTransform,
                eventData.position, this.canvas.worldCamera, out var delta);

            if (success)
            {
                this.windowRectTransform.anchoredPosition += eventData.delta / this.canvas.scaleFactor;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.SetLastPositionInHierarchy();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        this.SetLastPositionInHierarchy();
        return;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        return;
    }

    public void SetLastPositionInHierarchy()
    {
        this.transform.SetSiblingIndex(this.transform.parent.childCount - 1);
    }

    private void AnimatedHide()
    {
        this.windowRectTransform.DOSizeDelta(new Vector2(this.windowRectTransform.sizeDelta.x, this.startWindowDeltaSize.y),
            openWindowDuration).SetEase(Ease.OutQuad);
    }

    private void AnimatedUnhide()
    {
        this.windowRectTransform.DOSizeDelta(new Vector2(this.windowRectTransform.sizeDelta.x, this.windowDeltaSizeAfterChange.y),
            openWindowDuration).SetEase(Ease.OutQuad);
    }
}
