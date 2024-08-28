using System.Collections.Generic;
using UnityEngine;

public class UnitTypeController : MonoBehaviour
{
    public List<GameObject> unitsAllType;
    public GameObject unit;
    public int unitNumber;
    public Canvas baseMode;
    public Canvas attackMode;
    public GameObject sideController;

    private int lastHiredIndex = -1; // Трек индекса последнего объекта с тегом "Hired"
    private int lastHired2Index = -1; // Трек индекса последнего объекта с тегом "Hired2"
    private string lastSelectionTag = "NoneUnit"; // Трек последнего выбранного тега

    void Update()
    {
        // Если активирован baseMode, устанавливаем тег Hired у sideController
        if (baseMode.isActiveAndEnabled)
        {
            sideController.tag = "Hired";
        }

        // Если активирован attackMode
        if (attackMode.isActiveAndEnabled)
        {
            // Проверяем родительский тег каждого объекта в unitsAllType
            foreach (GameObject obj in unitsAllType)
            {
                Transform parentTransform = obj.transform.parent;
                if (parentTransform != null)
                {
                    if (parentTransform.CompareTag("DemocraticAliance"))
                    {
                        sideController.tag = "Users";
                    }
                    else
                    {
                        sideController.tag = "Enemy";
                    }
                }
            }
        }

        // Далее идет остальная часть вашего кода для обработки других условий...
        if (unitNumber == 1)
        {
            int notHiredCount = 0;
            bool hasHired = false;
            int newHiredIndex = -1;

            foreach (GameObject obj in unitsAllType)
            {
                if (obj.CompareTag("NotHired"))
                {
                    notHiredCount++;
                }
                else if (obj.CompareTag("Hired"))
                {
                    hasHired = true;
                    newHiredIndex = unitsAllType.IndexOf(obj);
                }
            }

            // Если все 6 объектов имеют тег "NotHired"
            if (notHiredCount == 6)
            {
                unit.tag = "NoneUnit";
                gameObject.tag = "NoneUnit";
                lastHiredIndex = -1; // Сброс отслеживаемого индекса
                lastSelectionTag = "NoneUnit"; // Обновление последнего тега выбора
                return; // Пропускаем дальнейшую обработку
            }

            if (hasHired)
            {
                string newTag = GetTagFromIndex(newHiredIndex);
                if (newTag == lastSelectionTag || lastSelectionTag == "NoneUnit")
                {
                    SetUnitTag(newTag);
                }
                lastHiredIndex = newHiredIndex; // Обновление последнего индекса с тегом Hired
            }
        }
        else if (unitNumber == 2)
        {
            int notHiredCount = 0;
            int hiredCount = 0;
            bool hasHired2 = false;
            int newHiredIndex = -1;
            int newHired2Index = -1;

            foreach (GameObject obj in unitsAllType)
            {
                if (obj.CompareTag("NotHired"))
                {
                    notHiredCount++;
                }
                else if (obj.CompareTag("Hired"))
                {
                    hiredCount++;
                    newHiredIndex = unitsAllType.IndexOf(obj);
                }
                else if (obj.CompareTag("Hired2"))
                {
                    hasHired2 = true;
                    newHired2Index = unitsAllType.IndexOf(obj);
                }
            }

            // Если ровно 5 объектов имеют тег "NotHired" и 1 объект имеет тег "Hired"
            if ((notHiredCount == 5 && hiredCount == 1) || notHiredCount == 6)
            {
                unit.tag = "NoneUnit";
                gameObject.tag = "NoneUnit";
                lastHiredIndex = -1; // Сброс отслеживаемого индекса
                lastHired2Index = -1; // Сброс последнего индекса с тегом Hired2
                lastSelectionTag = "NoneUnit"; // Обновление последнего тега выбора
                return; // Пропускаем дальнейшую обработку
            }

            if (hiredCount == 2 || hasHired2)
            {
                string newTag;
                if (newHired2Index != -1)
                {
                    newTag = GetTagFromIndex(newHired2Index);
                }
                else
                {
                    newTag = GetTagFromIndex(newHiredIndex);
                }

                if (newTag == lastSelectionTag || lastSelectionTag == "NoneUnit")
                {
                    SetUnitTag(newTag);
                }

                lastHiredIndex = newHiredIndex; // Обновление последнего индекса с тегом Hired
                lastHired2Index = newHired2Index; // Обновление последнего индекса с тегом Hired2
            }
        }
    }

    private string GetTagFromIndex(int index)
    {
        switch (index)
        {
            case 0: return "Shooter";
            case 1: return "Stormtrooper";
            case 2: return "ArmouredInfantryman";
            case 3: return "Flamethrower";
            case 4: return "Scout";
            case 5: return "Rifleman";
            default: return "Unknown"; // Значение по умолчанию, если индекс выходит за пределы диапазона
        }
    }

    private void SetUnitTag(string tag)
    {
        if (tag == "Unknown")
        {
            tag = "NoneUnit"; // Резервный вариант, если тег неизвестен
        }

        gameObject.tag = tag;

        // Устанавливаем тег объекта unit на основе тега gameObject
        if (tag == "Hired")
        {
            // Разрешаем изменение тега unit с "Hired" на "Users" или "Enemy"
            unit.tag = (unit.tag == "Hired") ? unit.tag : "Hired";
        }
        else if (tag == "Shooter" || tag == "Stormtrooper" || tag == "ArmouredInfantryman" ||
                 tag == "Flamethrower" || tag == "Scout" || tag == "Rifleman")
        {
            // Для этих специфических тегов устанавливаем тег unit на "Hired"
            unit.tag = "Hired";
        }
        else
        {
            // Для всех остальных тегов тег объекта unit будет соответствовать тегу gameObject
            unit.tag = tag;
        }

        lastSelectionTag = tag; // Обновление последнего тега выбора
    }

    private bool HasAnotherHired()
    {
        int hiredCount = 0;
        foreach (GameObject obj in unitsAllType)
        {
            if (obj.CompareTag("Hired"))
            {
                hiredCount++;
                if (hiredCount > 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
