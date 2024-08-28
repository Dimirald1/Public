using System.Collections;
using UnityEngine;

public class ClickPoint : MonoBehaviour
{
    public GameObject targetObject; // Ссылка на объект, который будет двигаться
    public float vanishDuration = 1f;  // Время, за которое объект исчезает
    public float maxScale = 0.04f;     // Максимальный размер объекта

    private Coroutine currentAnimationCoroutine; // Текущая корутина анимации

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("ClickPoint должен быть прикреплен к объекту в сцене!");
            return;
        }
        // Убедимся, что объект всегда активен
        targetObject.SetActive(true);
        targetObject.transform.localScale = Vector3.zero;
    }

    public void HandleClick(Vector3 targetPosition)
    {
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine); // Прерываем текущую корутину, если она существует
        }
        currentAnimationCoroutine = StartCoroutine(MoveAndAnimateObject(targetPosition));
    }

    private IEnumerator MoveAndAnimateObject(Vector3 targetPosition)
    {
        // Перемещаем объект к позиции клика
        targetObject.transform.position = targetPosition;
        targetObject.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        // Устанавливаем начальный и конечный размер
        Vector3 startScale = new Vector3(maxScale, maxScale, maxScale);
        Vector3 endScale = Vector3.zero;

        // Анимация изменения размера объекта
        float elapsed = 0f;
        while (elapsed < vanishDuration)
        {
            float t = elapsed / vanishDuration;
            targetObject.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Убедимся, что объект имеет конечный размер
        targetObject.transform.localScale = endScale;
    }
}
