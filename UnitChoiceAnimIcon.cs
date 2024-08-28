using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UnitChoiceManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDeselectHandler, IPointerDownHandler
{
    public Image iconImage;          // Иконка на кнопке
    public Sprite defaultIcon;       // Иконка по умолчанию
    public Sprite selectedIcon;      // Иконка, когда кнопка выбрана
    public Color hoverColor;         // Цвет при наведении курсора
    private Color initialColor;      // Начальный цвет кнопки

    private Button button;           // Компонент Button
    private static UnitChoiceManager selectedButton; // Ссылка на выбранную кнопку

    public CurrencyManager currencyManager; // Ссылка на объект CurrencyManager
    public List<Button> units;          // Список кнопок
    public List<float> unitCosts;       // Список стоимостей кнопок
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;

    public List<GameObject> windows; // Новый список окон
    public OpenCloseUnitChoice openCloseUnitChoice; // Новый объект для управления открытием/закрытием
    public Text maxHiredText;         // Новый публичный текстовый объект
    public float delay = 0.2f; // Задержка перед деактивацией кнопок

    private Coroutine deactivateCoroutine; // Корутин для деактивации кнопок

    void Start()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            initialColor = button.image.color; // Запоминаем начальный цвет кнопки
        }

        if (iconImage != null)
        {
            iconImage.sprite = defaultIcon; // Устанавливаем иконку по умолчанию
        }

        // Устанавливаем начальный тег
        SetButtonTag(false);

        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager не установлен!");
        }

        // Проверка количества кнопок и стоимостей
        if (units.Count != unitCosts.Count)
        {
            Debug.LogError("Количество кнопок не совпадает с количеством стоимостей!");
        }

        // Первоначальная установка состояния кнопок
        UpdateButtonState();
    }

    void Update()
    {
        // Проверяем состояние кнопок и обновляем состояние кнопок
        UpdateButtonState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Меняем цвет кнопки при наведении курсора
        if (button != null && selectedButton != this)
        {
            button.image.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Возвращаем начальный цвет кнопки, если курсор покидает ее и кнопка не выбрана
        if (button != null && selectedButton != this)
        {
            button.image.color = initialColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Если уже выбрана эта кнопка, ничего не делаем
        if (selectedButton == this)
            return;

        // Сбрасываем состояние предыдущей выбранной кнопки
        if (selectedButton != null)
        {
            selectedButton.DeselectButton();
        }

        // Устанавливаем новое состояние выбранной кнопки
        selectedButton = this;
        if (button != null)
        {
            button.image.color = initialColor; // Возвращаем начальный цвет кнопки
        }

        if (iconImage != null)
        {
            iconImage.sprite = selectedIcon; // Устанавливаем выбранную иконку
        }

        // Устанавливаем тег в "Selected"
        SetButtonTag(true);

        // Обновляем состояние кнопок в ConfirmCanBeUsed
        UpdateButtonState();
    }

    public void DeselectButton()
    {
        if (button != null)
        {
            button.image.color = initialColor; // Возвращаем начальный цвет кнопки
        }

        if (iconImage != null)
        {
            iconImage.sprite = defaultIcon; // Устанавливаем иконку по умолчанию
        }

        // Устанавливаем тег в "NotSelected"
        SetButtonTag(false);

        // Обновляем состояние кнопок в ConfirmCanBeUsed
        UpdateButtonState();
    }

    private void SetButtonTag(bool isSelected)
    {
        string tag = isSelected ? "Selected" : "NotSelected";
        gameObject.tag = tag;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Сброс состояния при клике вне кнопки
        if (selectedButton == this)
        {
            selectedButton = null;
            DeselectButton();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Сразу устанавливаем кнопку как выбранную при удержании клика
        OnPointerClick(eventData);
    }

    public void UpdateButtonState()
    {
        if (units.Count == 0)
        {
            return;
        }

        // Останавливаем текущую корутину деактивации, если она существует
        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
            deactivateCoroutine = null;
        }

        // Проверяем новый публичный Text объект
        bool maxHiredTextFlag = false;

        if (maxHiredText != null && maxHiredText.CompareTag("MaxHired"))
        {
            maxHiredTextFlag = true;
        }

        // Ищем кнопку с тегом Selected
        Button buttonToActivate = null;
        bool maxHired = false;

        for (int i = 0; i < windows.Count; i++)
        {
            GameObject window = windows[i];
            if (openCloseUnitChoice.currentIndex == i)
            {
                Transform unitsCountTransform = window.transform.Find("UnitsCount");
                if (unitsCountTransform != null)
                {
                    if (unitsCountTransform.CompareTag("MaxHired"))
                    {
                        maxHired = true;
                        break;
                    }
                }
            }
        }

        if (maxHired || maxHiredTextFlag)
        {
            // Если есть окно с тегом MaxHired или текстовый объект с тегом MaxHired, запретить использование тега Selected
            foreach (Button btn in units)
            {
                if (btn.gameObject.CompareTag("Selected"))
                {
                    btn.gameObject.tag = "NotSelected";
                }
            }
        }

        for (int i = 0; i < units.Count; i++)
        {
            Button btn = units[i];
            float cost = unitCosts[i];

            if (btn != null && btn.gameObject.CompareTag("Selected"))
            {
                if (currencyManager.playerBalance >= cost)
                {
                    // Определяем, какая кнопка должна быть активирована
                    switch (i)
                    {
                        case 0:
                            buttonToActivate = button1;
                            break;
                        case 1:
                            buttonToActivate = button2;
                            break;
                        case 2:
                            buttonToActivate = button3;
                            break;
                        case 3:
                            buttonToActivate = button4;
                            break;
                        case 4:
                            buttonToActivate = button5;
                            break;
                        case 5:
                            buttonToActivate = button6;
                            break;
                        default:
                            Debug.LogWarning("Index out of range: " + i);
                            break;
                    }

                    // Прерываем цикл после нахождения первой кнопки с тегом Selected
                    break;
                }
            }
        }

        // Активируем только нужную кнопку и перемещаем её на передний план
        if (buttonToActivate != null)
        {
            DeactivateAllButtons();
            buttonToActivate.gameObject.SetActive(true);
            buttonToActivate.transform.SetAsLastSibling();
        }
        else
        {
            // Запускаем корутину деактивации всех кнопок с задержкой
            deactivateCoroutine = StartCoroutine(DeactivateButtonsWithDelay());
        }
    }

    IEnumerator DeactivateButtonsWithDelay()
    {
        // Задержка перед деактивацией
        yield return new WaitForSeconds(delay);

        // Деактивируем все кнопки
        DeactivateAllButtons();

        Debug.Log("All buttons deactivated after delay");
    }

    void DeactivateAllButtons()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
        button5.gameObject.SetActive(false);
        button6.gameObject.SetActive(false);
    }
}