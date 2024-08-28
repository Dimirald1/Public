using UnityEngine;
using UnityEngine.UI;

public class SelectedHandler : MonoBehaviour
{
    public Button yourButton; // Reference to the button
    public CurrencyManager currencyManager; // Reference to CurrencyManager
    public float count; // Cost to deduct
    public GameObject tagObject; // The object whose tag will be checked
    public GameObject objectToHide; // The object that will be hidden if the tag is "NotSelected"

    void Update() 
    {
        if (tagObject != null && tagObject.tag == "NotSelected")
        {
            // Hide the additional public object if the tag is "NotSelected"
            if (objectToHide != null)
            {
                objectToHide.SetActive(false);
            }
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("OnButtonClick called");
        if (currencyManager == null)
        {
            Debug.LogError("currencyManager is not assigned.");
            return;
        }

        Debug.Log("Current player balance: " + currencyManager.playerBalance);
        Debug.Log("Cost to deduct: " + count);

        if (currencyManager.playerBalance >= count)
        {
            Debug.Log("Sufficient balance. Proceeding to deduct.");

            // Deduct the cost from the balance
            currencyManager.AddToBalance(-count);

            // Deactivate the button
            yourButton.gameObject.SetActive(false);

            Debug.Log("Balance updated. New balance: " + currencyManager.playerBalance);
        }
        else
        {
            Debug.Log("Not enough balance.");
        }
    }
}