namespace ExoLab.UI
{
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// ѕозвол€ет оптимизированно работать с текстовым элементом внутри него.
    /// Ќе нужно каждый раз искать текстовое поле в дет€х, перебира€ варианты
    /// </summary>
    public class TextDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public void SetText(string value)
        {
            this.text.text = value;
        }
    }
}
