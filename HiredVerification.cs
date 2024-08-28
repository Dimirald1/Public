using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiredVerification : MonoBehaviour
{
    public Image targetImage;         // Публичный объект с компонентом Image
    public Sprite hiredSprite;        // Картинка для тега Hired
    public Sprite notHiredSprite;     // Картинка для тега NotHired
    public GameObject imageUnit;      // Публичный объект для управления активностью
    public GameObject hpObject1;
    public GameObject hpObject2;
    public GameObject hpBar;
    public OpenCloseUnitChoice openCloseUnitChoice;
    public GameObject expensesObject;  // Object for managing expenses
    public float expenses;

    private string lastTag;           // Переменная для хранения предыдущего тега

    void Start()
    {
        // Проверяем, что целевой объект для картинки задан в инспекторе
        if (targetImage == null)
        {
            Debug.LogError("Целевой объект с компонентом Image не задан.");
            return;
        }

        // Проверяем, что все публичные объекты и спрайты заданы
        if (hiredSprite == null || notHiredSprite == null)
        {
            Debug.LogError("Один или несколько спрайтов не заданы.");
        }

        lastTag = gameObject.tag;     // Сохраняем начальный тег
        UpdateImage();                // Выполняем первоначальное обновление на старте
    }

    void Update()
    {
        if (targetImage == null) return; // Прекращаем выполнение, если targetImage не задан

        if (gameObject.tag != lastTag) // Проверяем, изменился ли тег
        {
            lastTag = gameObject.tag;  // Обновляем сохраненный тег
            UpdateImage();             // Обновляем изображение и активность объекта
        }
    }

    void ExpensesMinus()
    {
        if (expensesObject != null && openCloseUnitChoice != null)
        {
            int currentIndex = openCloseUnitChoice.currentIndex;
            if (currentIndex >= 0 && currentIndex < expensesObject.transform.childCount)
            {
                Transform expenseTransform = expensesObject.transform.GetChild(currentIndex);
                Expenses expensesComponent = expenseTransform.GetComponent<Expenses>();
                if (expensesComponent != null)
                {
                    expensesComponent.expensesValue -= expenses;
                    Debug.Log("Expenses updated. New expenses value: " + expensesComponent.expensesValue);
                }
                else
                {
                    Debug.LogError("Expenses component not found on the child object.");
                }
            }
            else
            {
                Debug.LogError("Invalid index for expensesObject children.");
            }
        }
    }

    void UpdateImage()
    {
        if (targetImage == null)
        {
            Debug.LogError("Целевой объект с компонентом Image не задан.");
            return;
        }

        // Устанавливаем картинку и активность объекта в зависимости от тега
        switch (gameObject.tag)
        {
            case "Hired":
                targetImage.sprite = hiredSprite;
                targetImage.color = Color.white; // Убедитесь, что цвет не изменен
                imageUnit.SetActive(true);
                hpObject1.SetActive(true);
                hpObject2.SetActive(false);
                hpBar.SetActive(true);
                break;
            case "Hired2":
                targetImage.sprite = hiredSprite;
                targetImage.color = Color.white; // Убедитесь, что цвет не изменен
                imageUnit.SetActive(true);
                hpObject1.SetActive(true);
                hpObject2.SetActive(true);
                hpBar.SetActive(true);
                break;
            case "NotHired":
                targetImage.sprite = notHiredSprite;
                targetImage.color = Color.white; // Убедитесь, что цвет не изменен
                imageUnit.SetActive(false);
                hpObject1.SetActive(false);
                hpObject2.SetActive(false);
                hpBar.SetActive(false);
                break;
            case "AtTheOtherStation":
                targetImage.sprite = notHiredSprite;
                targetImage.color = Color.white; // Убедитесь, что цвет не изменен
                imageUnit.SetActive(false);
                hpObject1.SetActive(false);
                hpObject2.SetActive(false);
                hpBar.SetActive(false);
                ExpensesMinus();
                gameObject.tag = "NotHired";
                break;
            case "InTheAttack":
                targetImage.color = new Color(1f, 1f, 1f, 0f);  // Устанавливаем альфа-канал в 0
                imageUnit.SetActive(true);
                hpObject1.SetActive(true);
                hpBar.SetActive(true);
                break;
            case "InTheAttack2":
                targetImage.color = new Color(1f, 1f, 1f, 0f);  // Устанавливаем альфа-канал в 0
                imageUnit.SetActive(true);
                hpObject1.SetActive(true);
                hpObject2.SetActive(true);
                hpBar.SetActive(true);
                break;
            default:
                Debug.LogWarning($"Неизвестный тег объекта: {gameObject.tag}");
                break;
        }
    }
}