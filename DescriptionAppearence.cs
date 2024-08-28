using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DescriptionAppearence : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip; // Объект изображения, который будет показываться
    private Coroutine tooltipCoroutine; // Хранение ссылки на корутину

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipCoroutine = StartCoroutine(ShowTooltip());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipCoroutine != null)
        {
            StopCoroutine(tooltipCoroutine); // Остановить корутину
            tooltipCoroutine = null; // Обнулить ссылку на корутину
        }
        tooltip.SetActive(false); // Скрыть тултип
    }

    private IEnumerator ShowTooltip()
    {
        yield return new WaitForSeconds(0.6f);
        tooltip.SetActive(true);
    }
}