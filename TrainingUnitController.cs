using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingUnitController : MonoBehaviour
{
    // Публичный список спрайтов
    public List<Sprite> sprites = new List<Sprite>();

    // Публичное значение short, которое будет изменять изображение
    [Range(0, 6)]
    public short imageIndex = 0;

    // Ссылка на компонент Image для изменения его спрайта
    private Image currentImage;

    void Start()
    {
        // Инициализация текущего изображения, если оно существует
        currentImage = GetComponent<Image>();
        UpdateImage();
    }

    void Update()
    {
        // Обновляем изображение при изменении imageIndex
        UpdateImage();
    }

    public void UpdateImage()
    {
        // Проверяем, чтобы значение imageIndex не выходило за пределы допустимого диапазона
        if (imageIndex >= 0 && imageIndex < sprites.Count)
        {
            // Устанавливаем спрайт текущего изображения на основе imageIndex
            if (currentImage != null)
            {
                currentImage.sprite = sprites[imageIndex];

                // Управление прозрачностью в зависимости от imageIndex
                if (imageIndex == 0)
                {
                    // Устанавливаем прозрачность на 0 (полностью прозрачный)
                    Color transparent = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 0);
                    currentImage.color = transparent;
                }
                else
                {
                    // Устанавливаем непрозрачность на 100% (полностью видимый)
                    Color opaque = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 1);
                    currentImage.color = opaque;
                }
            }
        }
        else
        {
            Debug.LogWarning("imageIndex выходит за пределы допустимого диапазона!");
        }
    }
}