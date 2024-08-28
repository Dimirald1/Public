using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PercentageCalculator : MonoBehaviour
{
    [Header("Percent Texts")]
    public List<Text> percentTexts; // Список текстов процентов

    [Header("Values")]
    public List<float> values; // Список значений

    [Header("Output Text")]
    public Text outputText; // Текст для вывода суммы

    [Header("Currency Manager")]
    public CurrencyManager currencyManager; // Ссылка на CurrencyManager
    private float calculatedSum = 0; // Сумма после вычислений

    [Header("HP Units")]
    public List<HPUnitDisplaying> hpUnits; // Список объектов с компонентом HPUnitDisplaying

    [Header("Tag Check Objects")]
    public List<GameObject> tagCheckObjects; // Список объектов для проверки тегов

    void Update()
    {
        CalculateSum();
        UpdateOutputTextVisibility(); // Обновление отображения текста на основе тегов объектов
    }

    void CalculateSum()
    {
        calculatedSum = 0;
        for (int i = 0; i < percentTexts.Count; i++)
        {
            // Проверяем, активен ли текстовый объект
            if (percentTexts[i].gameObject.activeInHierarchy && i < values.Count)
            {
                // Удаляем символ % и пробелы из строки
                string text = percentTexts[i].text.Replace("%", "").Trim();
                
                // Проверка на пустую строку
                if (string.IsNullOrEmpty(text))
                {
                    continue;
                }

                // Пробуем преобразовать строку в float
                if (float.TryParse(text, out float percentage))
                {
                    percentage = 100f - percentage;
                    calculatedSum += (percentage / 100f) * values[i];
                }
            }
        }
    }

    // Метод для обновления отображения текста на основе тегов объектов
    void UpdateOutputTextVisibility()
    {
        bool inTheAttack = false;
        bool hired = false;

        foreach (var obj in tagCheckObjects)
        {
            if (obj.CompareTag("InTheAttack") || obj.CompareTag("InTheAttack2"))
            {
                inTheAttack = true;
            }
            if (obj.CompareTag("Hired") || obj.CompareTag("Hired2"))
            {
                hired = true;
            }
        }

        // Если сумма равна 0, скрываем текст
        if (calculatedSum == 0)
        {
            outputText.gameObject.SetActive(false);
        }
        // Если хотя бы у одного объекта тег InTheAttack, скрываем текст
        else if (inTheAttack)
        {
            outputText.gameObject.SetActive(false);
        }
        // Если хотя бы у одного объекта тег Hired, показываем текст
        else if (hired)
        {
            outputText.gameObject.SetActive(true);
            outputText.text = calculatedSum.ToString("F0") + " M-coin";
        }
        else
        {
            outputText.gameObject.SetActive(false);
        }
    }

    // Метод, который можно привязать к кнопке
    public void PerformAction()
    {
        CalculateSum(); // Убедимся, что сумма пересчитана перед выполнением действия

        if (currencyManager != null)
        {
            if (calculatedSum <= currencyManager.playerBalance)
            {
                currencyManager.playerBalance -= calculatedSum;

                // Обновление здоровья у всех объектов из списка hpUnits
                foreach (var hpUnit in hpUnits)
                {
                    hpUnit.HP = hpUnit.maxHealth;
                    hpUnit.UpdateHealthBar(); // Обновляем отображение здоровья
                }
            }
            else
            {
                // Здесь можно добавить код для отображения сообщения пользователю
                // Например, показать уведомление или изменить текст на UI
            }
        }
    }
}