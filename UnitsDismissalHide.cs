using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitsDismissalHide : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tag Check Object")]
    public GameObject tagCheckObject; // Объект для проверки тега

    [Header("Button")]
    public GameObject dismissalButton; // Кнопка для увольнения

    [Header("HP Unit Displaying Objects")]
    public GameObject firstHPUnitDisplayingObject; // Первый объект HPUnitDisplaying
    public GameObject secondHPUnitDisplayingObject; // Второй объект HPUnitDisplaying
    public OpenCloseUnitChoice openCloseUnitChoice;
    public Expenses expensesComponent; // Объект для управления расходами
    public float expenses; // Расходы

    private Coroutine showButtonCoroutine;

    void Start()
    {
        if (dismissalButton != null)
        {
            dismissalButton.SetActive(false);
        }
        else
        {
            Debug.LogError("Кнопка dismissalButton не назначена!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (showButtonCoroutine != null)
        {
            StopCoroutine(showButtonCoroutine);
        }
        showButtonCoroutine = StartCoroutine(ShowButtonWithDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showButtonCoroutine != null)
        {
            StopCoroutine(showButtonCoroutine);
        }
        if (dismissalButton != null)
        {
            dismissalButton.SetActive(false);
        }
    }

    private IEnumerator ShowButtonWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        if (tagCheckObject != null && (tagCheckObject.CompareTag("Hired") || tagCheckObject.CompareTag("Hired2")))
        {
            if (dismissalButton != null)
            {
                dismissalButton.SetActive(true);
            }
        }
    }

    public void CheckAndChangeTag(GameObject targetObject)
    {
        if (targetObject == null)
        {
            Debug.LogError("targetObject не назначен!");
            return;
        }

        string currentTag = targetObject.tag;

        HPUnitDisplaying firstHPUnit = firstHPUnitDisplayingObject?.GetComponent<HPUnitDisplaying>();
        HPUnitDisplaying secondHPUnit = secondHPUnitDisplayingObject?.GetComponent<HPUnitDisplaying>();

        if (firstHPUnit != null && secondHPUnit != null)
        {
            float firstHP = firstHPUnit.HP;
            float secondHP = secondHPUnit.HP;

            if (currentTag == "Hired" || currentTag == "InTheAttack")
            {
                targetObject.tag = "NotHired";
                ExpensesMinus();
            }
            else if (currentTag == "Hired2")
            {
                ExpensesMinus();
                if (firstHP > secondHP)
                {
                    targetObject.tag = "Hired";
                }
                else
                {
                    targetObject.tag = "Hired";
                    if (firstHP != secondHP)
                    {
                        firstHPUnit.HP = (short)Mathf.RoundToInt(secondHP);
                        firstHPUnit.UpdateHealthBar();

                        secondHPUnit.HP = secondHPUnit.maxHealth;
                        secondHPUnit.UpdateHealthBar();
                    }
                }
            }
            else if (currentTag == "InTheAttack2")
            {
                ExpensesMinus();
                if (firstHP > secondHP)
                {
                    targetObject.tag = "InTheAttack";
                }
                else
                {
                    targetObject.tag = "InTheAttack";
                    if (firstHP != secondHP)
                    {
                        firstHPUnit.HP = (short)Mathf.RoundToInt(secondHP);
                        firstHPUnit.UpdateHealthBar();

                        secondHPUnit.HP = secondHPUnit.maxHealth;
                        secondHPUnit.UpdateHealthBar();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Компоненты HPUnitDisplaying не назначены или отсутствуют!");
        }

        if (dismissalButton != null)
        {
            dismissalButton.SetActive(false);
        }
    }

    void ExpensesMinus()
    {
                if (expensesComponent != null)
                {
                    expensesComponent.expensesValue -= expenses;
                    Debug.Log("Расходы обновлены. Новое значение расходов: " + expensesComponent.expensesValue);
                }
                else
                {
                    Debug.LogError("Компонент Expenses не найден на дочернем объекте.");
                }
    }
}

