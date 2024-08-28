using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUnitDisplaying : MonoBehaviour
{
    public short maxHealth;
    public short HP;
    public GameObject targetObject;
    private Image healthBar;
    public Text hpPercentageText;

    public UnitsDismissalHide unitsDismissalHide;




    void Start()
    {
        healthBar = GetComponent<Image>();

        // Check for null to avoid NullReferenceException
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

        HP = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        // Check for null before performing operations
        if (healthBar == null || hpPercentageText == null)
        {
            return;
        }

        // Ensure HP does not exceed maxHealth
        if (HP > maxHealth)
        {
            HP = maxHealth;
        }

        // Update the health bar fill amount
        float fillAmount = (float)HP / maxHealth;
        healthBar.fillAmount = fillAmount;

        // Update the percentage text
        int percentage = Mathf.RoundToInt(fillAmount * 100);
        hpPercentageText.text = percentage.ToString() + "%";
        hpPercentageText.color = Color.white;

        // Check if HP is 0 and update the tag
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

    // Method to modify HP (e.g., from external scripts)
    public void ModifyHP(short amount)
    {
        HP += amount;

        // Ensure HP stays within the valid range
        if (HP > maxHealth)
        {
            HP = maxHealth;
        }
        else if (HP < 0)
        {
            HP = 0;
        }

        UpdateHealthBar();
    }
}
