using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHideOrNo : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Получаем компонент SpriteRenderer при старте
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            // Проверяем тег объекта и устанавливаем прозрачность
            switch (gameObject.tag)
            {
                case "NoneUnit":
                    SetTransparency(0f); // Полностью прозрачный
                    break;
                case "Hired":
                    SetTransparency(1f); // Полностью непрозрачный
                    break;
                default:
                    // Если тег не подходит, можно либо оставить текущую прозрачность, либо задать значение по умолчанию
                    SetTransparency(1f); // Можно установить по умолчанию непрозрачность
                    break;
            }
        }
    }

    private void SetTransparency(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha; // Устанавливаем значение альфа-канала
            spriteRenderer.color = color;
        }
    }
}