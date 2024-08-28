using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanOpenUnitChoice : MonoBehaviour
{
    public GameObject targetObject; // Публичный объект, тег которого будет проверяться
    public Button button; // Кнопка, функционал которой будет блокироваться или разблокироваться

    void Start()
    {
        // Сохраняем оригинальные слушатели кнопки
        Button.ButtonClickedEvent originalListeners = button.onClick;

        // Очищаем слушатели и добавляем наш кастомный
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnButtonClick(originalListeners));
    }

    void OnButtonClick(Button.ButtonClickedEvent originalListeners)
    {
        if (targetObject.tag == "Training")
        {
            // Если тег "Training", то блокируем функционал кнопки
            Debug.Log("Button is disabled because the tag is Training.");
            return;
        }

        if (targetObject.tag == "NotTraining")
        {
            // Если тег "NotTraining", выполняем основную логику
            originalListeners.Invoke();
        }
    }

    void PerformButtonAction()
    {
        // Ваша основная логика кнопки, если необходимо
        Debug.Log("Button clicked and action performed.");
    }
}