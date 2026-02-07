using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotatingSrollBar : MonoBehaviour
{
    [SerializeField] private RotatingObject @object;

    [SerializeField] private int currentValue;
    [SerializeField] private int maxValue;

    private TextMeshProUGUI text;
    private Slider slider;

    private void Start()
    {
        this.Init();
        this.DisplayInfo();
    }

    public void OnUpdate()
    {
        var value = this.slider.value;

        if (value >= 0 && value < maxValue)
        {
            this.@object.rotationSpeed = value;
            this.DisplayInfo();
        }
    }

    private void Init()
    {
        this.text = this.GetComponentInChildren<TextMeshProUGUI>();
        this.slider = this.GetComponentInChildren<Slider>();

        this.slider.minValue = 0;
        this.slider.maxValue = maxValue;
    }

    private void DisplayInfo()
    {
        this.text.text = $"{this.@object.rotationSpeed} r/m";
        //this.slider.value = this.@object.rotationSpeed / maxValue;
    }
}
