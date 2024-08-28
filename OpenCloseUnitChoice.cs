using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseUnitChoice : MonoBehaviour
{
    // Публичный список объектов
    public GameObject[] publicList;

    // Индекс окна
    public int currentIndex; // Изменено с indexWindow на currentIndex

    // Метод, вызываемый кнопкой
    public void OpenUnitChoice(int indexWindow)
    {
        // Устанавливаем currentIndex в переданное значение
        this.currentIndex = indexWindow;

        // Проверяем, что список не пуст и индексWindow находится в пределах списка
        if (publicList != null && publicList.Length > 0 && indexWindow >= 0 && indexWindow < publicList.Length)
        {
            // Связываем объект с индексом из списка
            GameObject selectedObject = publicList[indexWindow];

            // Выводим информацию в консоль для проверки
            Debug.Log($"Объект {selectedObject.name} связан с индексом {indexWindow}");

            // Проверяем, что объект не равен null перед активацией
            if (selectedObject != null)
            {
                // Активируем выбранный объект
                gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Объект, связанный с индексом, равен null.");
            }
        }
        else
        {
            Debug.LogWarning("Неверный индекс или пустой список.");
        }
    }
}