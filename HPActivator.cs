using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPActivator : MonoBehaviour
{
    public GameObject targetObject;  // Объект для проверки тега
    public string targetTag = "Hired";  // Тег для проверки

    private bool isActive;  // Переменная для отслеживания текущего состояния

    void Start()
    {
        // Изначально проверяем состояние объекта
        CheckAndSetActive();
    }

    void Update()
    {
        // Постоянно проверяем изменение тега
        CheckAndSetActive();
    }

    private void CheckAndSetActive()
    {
        if (targetObject != null && targetObject.CompareTag(targetTag))
        {
            if (!isActive)
            {
                gameObject.SetActive(true);  // Активируем объект
                isActive = true;
            }
        }
        else
        {
            if (isActive)
            {
                gameObject.SetActive(false);  // Деактивируем объект
                isActive = false;
            }
        }
    }
}