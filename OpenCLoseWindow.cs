using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCLoseWindow : MonoBehaviour
{
    public GameObject window;

    public void ActivatePanel()
    {
        if (window != null)
        {
            window.SetActive(true); // Активируем панель
        }
    }
    
    public void DisactivatePanel()
    {
        if (window != null)
        {
            window.SetActive(false); // Активируем панель
        }
    }
}
