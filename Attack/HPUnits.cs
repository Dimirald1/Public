using UnityEngine;
using UnityEngine.UI;

public class HPUnits : MonoBehaviour
{
    public GameObject unit;
    public GameObject targetObject;
    private Image healthBar;
    public Text hpPercentageText;

    public UnitsDismissalHide unitsDismissalHide;

    private UnitsAttack unitsAttack; // Добавляем ссылку на UnitsAttack

    void Start()
    {
        // Получаем компонент UnitsAttack с того же объекта
        unitsAttack = GetComponent<UnitsAttack>();

        if (unitsAttack == null)
        {
            Debug.LogError("UnitsAttack component not found on this GameObject!");
            return;
        }

        healthBar = GetComponent<Image>();

        if (healthBar == null)
        {
            Debug.LogError("Image component not found on this GameObject!");
            return;
        }

        if (hpPercentageText == null)
        {
            Debug.LogError("hpPercentageText is not assigned!");
            return;
        }

        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        // Используем HP и maxHealth из UnitsAttack
        short HP = unitsAttack.hp;
        short maxHealth = unitsAttack.maxHealth;

        if (healthBar == null || hpPercentageText == null)
        {
            return;
        }

        if (HP > maxHealth)
        {
            HP = maxHealth;
        }

        // Обновляем заполнение полоски здоровья
        float fillAmount = (float)HP / maxHealth;
        healthBar.fillAmount = fillAmount;

        // Обновляем текст с процентами здоровья
        int percentage = Mathf.RoundToInt(fillAmount * 100);
        hpPercentageText.text = percentage.ToString() + "%";
        hpPercentageText.color = Color.white;

        // Проверяем, не равен ли HP 0, и обновляем тег
        if (HP <= 0)
        {
            if (targetObject.tag == "Hired" || targetObject.tag == "InTheAttack")
            {
                unitsDismissalHide.CheckAndChangeTag(targetObject);
            }
            if (targetObject.tag == "Hired2")
            {
                unitsDismissalHide.CheckAndChangeTag(targetObject);
            }
            if (targetObject.tag == "InTheAttack2")
            {
                unitsDismissalHide.CheckAndChangeTag(targetObject);
            }
        }
    }

    // Метод для изменения HP (например, из внешних скриптов)
    public void ModifyHP(short amount)
    {
        unitsAttack.hp += amount; // Используем HP из UnitsAttack

        if (unitsAttack.hp > unitsAttack.maxHealth)
        {
            unitsAttack.hp = unitsAttack.maxHealth;
        }
        else if (unitsAttack.hp < 0)
        {
            unitsAttack.hp = 0;
        }

        UpdateHealthBar();
    }
}
