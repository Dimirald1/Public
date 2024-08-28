using UnityEngine;
using UnityEngine.UI;

public class UnitReleaseController : MonoBehaviour
{
    public Button button;
    public UnityEngine.Events.UnityEvent[] functions;
    public int functionIndex; // Публичная переменная для изменения индекса

    void Start()
    {
        // Присваиваем метод OnButtonClick событию OnClick кнопки
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        if (functionIndex >= 0 && functionIndex < functions.Length)
        {
            functions[functionIndex]?.Invoke();
        }
        else
        {
            Debug.LogWarning("Индекс функции вне диапазона.");
        }
    }
}