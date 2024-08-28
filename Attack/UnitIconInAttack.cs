using System.Collections.Generic;
using UnityEngine;

public class UnitIconInAttack : MonoBehaviour
{
    public List<Sprite> onStation;
    public List<Sprite> users;
    public List<Sprite> enemy;
    public GameObject typeController;
    public GameObject sideController;
    public GameObject positionController;
    public Canvas baseMode;
    public Canvas attackMode;
    public GameObject baseSpawner;
    public GameObject attackSpawner;

    private Sprite defaultSprite;
    private CircleCollider2D circleCollider;

    void Start()
    {
        // Сохраняем начальный спрайт как базовый
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            defaultSprite = spriteRenderer.sprite;
        }

        // Получаем компонент CircleCollider2D
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
        {
            Debug.LogError("Компонент CircleCollider2D не найден на этом GameObject!");
        }

        AssignSprite();
    }

    void Update()
    {
        ManageCollider();
        AssignSprite();
    }

    void AssignSprite()
    {
        List<Sprite> selectedList = null;

        // Проверяем активный режим
        if (baseMode.gameObject.activeSelf)
        {
            if (sideController.tag == "Hired")
            {
                selectedList = onStation;

                // Проверяем тег positionController и перемещаем объект
                if (positionController.tag == "Hired")
                {
                    Vector3 newPosition = baseSpawner.transform.position;
                    transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
                }
            }
            else
            {
                // Если тег не "Hired", остаёмся с базовым спрайтом
                ResetToDefaultSprite();
                return;
            }
        }
        else if (attackMode.gameObject.activeSelf)
        {
            if (sideController.tag == "Users")
            {
                selectedList = users;
            }
            else if (sideController.tag == "Enemy")
            {
                selectedList = enemy;
            }
            else
            {
                // Если тег не "Users" и не "Enemy", остаёмся с базовым спрайтом
                ResetToDefaultSprite();
                return;
            }

            // Проверяем тег positionController и перемещаем объект
            if (positionController.tag == "Hired")
            {
                Vector3 newPosition = attackSpawner.transform.position;
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
        }
        else
        {
            Debug.LogError("Не обнаружен активный режим.");
            ResetToDefaultSprite();
            return;
        }


        // Проверяем тег positionController для "InTheAttack"
        if (positionController.tag == "InTheAttack")
        {
            // Не перемещаем объект, только меняем спрайт
            ChangeSprite(selectedList);
            return;
        }

        // Применяем спрайт
        ChangeSprite(selectedList);
    }

    void ManageCollider()
    {
        if (gameObject.tag == "NoneUnit")
        {
            if (circleCollider != null)
            {
                circleCollider.enabled = false;
            }
            ResetToDefaultSprite();
        }
        else if (gameObject.tag == "Hired")
        {
            if (circleCollider != null)
            {
                circleCollider.enabled = true;
            }
        }
    }

    void ChangeSprite(List<Sprite> selectedList)
    {
        if (selectedList == null)
        {
            Debug.LogError("Список выбранных спрайтов равен null");
            ResetToDefaultSprite();
            return;
        }

        Sprite selectedSprite = null;
        switch (typeController.tag)
        {
            case "NoneUnit":
                // Не назначаем спрайт, если тег NoneUnit
                ResetToDefaultSprite();
                return;
            case "Shooter":
                selectedSprite = GetSpriteFromList(selectedList, 0);
                break;
            case "Stormtrooper":
                selectedSprite = GetSpriteFromList(selectedList, 1);
                break;
            case "ArmouredInfantryman":
                selectedSprite = GetSpriteFromList(selectedList, 2);
                break;
            case "Flamethrower":
                selectedSprite = GetSpriteFromList(selectedList, 3);
                break;
            case "Scout":
                selectedSprite = GetSpriteFromList(selectedList, 4);
                break;
            case "Rifleman":
                selectedSprite = GetSpriteFromList(selectedList, 5);
                break;
            default:
                Debug.LogError("Неизвестный тег у typeController");
                ResetToDefaultSprite();
                return;
        }

        if (selectedSprite != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = selectedSprite;
            }
            else
            {
                Debug.LogError("Компонент SpriteRenderer не найден на этом GameObject!");
            }
        }
        else
        {
            ResetToDefaultSprite();
        }
    }

    Sprite GetSpriteFromList(List<Sprite> spriteList, int index)
    {
        return spriteList.Count > index ? spriteList[index] : null;
    }

    void ResetToDefaultSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }
}
