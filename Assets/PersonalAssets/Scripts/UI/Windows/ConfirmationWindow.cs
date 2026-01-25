using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Базовое окно выбора да/нет
/// </summary>
public class ConfirmationWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public void Show(string message, UnityAction onYesConfirmed, UnityAction onNoConfirmed = null)
    {
        this.gameObject.SetActive(true);
        this.titleText.text = message;

        // Сначала очищаем старые слушатели, чтобы методы не копились
        this.yesButton.onClick.RemoveAllListeners();
        this.noButton.onClick.RemoveAllListeners();

        // Подписываемся на кнопку "Да"
        this.yesButton.onClick.AddListener(() => {
            onYesConfirmed?.Invoke();
            Close();
        });

        // Подписываемся на кнопку "Нет"
        this.noButton.onClick.AddListener(() => {
            onNoConfirmed?.Invoke(); // Выполнится, если передали метод
            Close();
        });
    }

    private void Close()
    {
        this.gameObject.SetActive(false);
    }
}
