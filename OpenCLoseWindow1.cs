using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCLoseWindow1 : MonoBehaviour
{
    public GameObject window;
    public float deactivateDelay = 1.0f; // Delay time in seconds

    public void ActivatePanel()
    {
        if (window != null)
        {
            window.SetActive(true); // Activate the panel
        }
    }
    
    public void DisactivatePanel()
    {
        if (window != null)
        {
            StartCoroutine(DisactivatePanelWithDelay()); // Start coroutine to deactivate with delay
        }
    }

    private IEnumerator DisactivatePanelWithDelay()
    {
        yield return new WaitForSeconds(deactivateDelay); // Wait for the specified delay
        if (window != null)
        {
            window.SetActive(false); // Deactivate the panel
        }
    }
}