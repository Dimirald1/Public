using UnityEngine;
using UnityEngine.UI;

public class TextColorChanger : MonoBehaviour
{
    public Text targetText; // Текст, цвет которого будет изменяться
    private Color originalColor; // Исходный цвет текста

    void Start()
    {
        if (targetText != null)
        {
            originalColor = targetText.color; // Сохраняем исходный цвет текста
        }
        else
        {
            Debug.LogWarning("Target Text is not assigned.");
        }
    }

    // Публичная функция для изменения цвета текста
    public void ChangeTextColor()
    {
        ChangeTextColor("#050404");
    }

    public void ChangeTextColor(string colorCode)
    {
        if (targetText != null)
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString(colorCode, out newColor))
            {
                targetText.color = newColor; // Изменяем цвет текста на новый
            }
            else
            {
                Debug.LogError($"Invalid color code: {colorCode}");
            }
        }
        else
        {
            Debug.LogWarning("Target Text is not assigned.");
        }
    }

    // Публичная функция для восстановления исходного цвета текста
    public void ResetTextColor()
    {
        if (targetText != null)
        {
            targetText.color = originalColor; // Восстанавливаем исходный цвет текста
        }
        else
        {
            Debug.LogWarning("Target Text is not assigned.");
        }
    }
}