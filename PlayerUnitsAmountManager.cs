using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitsAmountManager : MonoBehaviour
{
    public List<GameObject> objects; // Список объектов для проверки
    public Text availableUnitsText; // Текст для отображения доступного количества юнитов
    public Text maxUnitsText; // Текст для отображения максимального количества юнитов

    private string[] unitTags = { "ArmouredInfantryman", "Scout", "Rifleman", "Stormtrooper", "Shooter", "Flamethrower" };

    // Start вызывается перед первым кадром
    void Start()
    {
        UpdateUnitCounts();
    }

    // Update вызывается каждый кадр
    void Update()
    {
        UpdateUnitCounts();
    }

    void UpdateUnitCounts()
    {
        int maxUnits = 0;
        int availableUnits = 0;

        // Вычисляем максимальное количество юнитов
        foreach (GameObject obj in objects)
        {
            if (obj.tag == "DemocraticAliance")
            {
                maxUnits++;
            }
        }

        // Вычисляем доступное количество юнитов
        foreach (GameObject obj in objects)
        {
            if (obj.tag == "DemocraticAliance")
            {
                foreach (string tag in unitTags)
                {
                    Transform child = obj.transform.Find(tag);
                    if (child != null)
                    {
                        if (child.tag == "Hired2" || child.tag == "InTheAttack2")
                        {
                            availableUnits += 2;
                        }
                        else if (child.tag == "Hired" || child.tag == "InTheAttack")
                        {
                            availableUnits++;
                        }
                    }
                }
            }
        }

        // Проверяем, не превышает ли доступное количество юнитов максимальное количество
        if (availableUnits > maxUnits)
        {
            availableUnits = maxUnits;
        }

        // Обновляем тексты
        maxUnitsText.text = maxUnits.ToString();
        availableUnitsText.text = availableUnits.ToString();

        // Обновляем тег текста с доступными юнитами
        if (availableUnits == 0)
        {
            availableUnitsText.tag = "NotHired";
        }
        else if (availableUnits < maxUnits)
        {
            availableUnitsText.tag = "Hired";
        }
        else if (availableUnits == maxUnits)
        {
            availableUnitsText.tag = "MaxHired";
        }
    }
}