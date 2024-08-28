using UnityEngine;
using UnityEngine.EventSystems;

public class CanRelease : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverObject;

    void Start()
    {
        // Ensure the hoverObject is hidden at the start
        if (hoverObject != null)
        {
            hoverObject.SetActive(false);
        }
    }
    void Update()
    {
        if (CompareTag("NotTraining") && hoverObject != null)
        {
            hoverObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CompareTag("Training") && hoverObject != null)
        {
            hoverObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CompareTag("Training") && hoverObject != null)
        {
            hoverObject.SetActive(false);
        }
    }
}