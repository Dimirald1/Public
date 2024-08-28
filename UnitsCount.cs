using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnitsCount : MonoBehaviour
{
    [Header("Tag Check Objects")]
    public List<GameObject> tagCheckObjects; // List of objects to check tags

    [Header("Output Text")]
    public Text outputText; // Text for displaying status

    void Update()
    {
        UpdateTagStatus();
    }

    void UpdateTagStatus()
    {
        int hiredCount = 0;
        GameObject hired2Object = null;
        List<GameObject> hiredObjects = new List<GameObject>();

        // Check for Hired2 and count Hired
        foreach (var obj in tagCheckObjects)
        {
            if (HasTag(obj, "Hired2", "InTheAttack2"))
            {
                hired2Object = obj;
                break;
            }

            if (HasTag(obj, "Hired", "InTheAttack"))
            {
                hiredObjects.Add(obj);
                hiredCount++;
            }
        }

        // System 3: If Hired2 is found, reset all other tags
        if (hired2Object != null)
        {
            SetOutputText("2/2");
            ResetOtherTags(hired2Object);
            SetOutputTextTag("MaxHired");
            return; // End execution since Hired2 has priority
        }

        // System 2: If there are 2 or more Hired, reset the excess
        if (hiredCount > 2)
        {
            ResetExcessHired(hiredObjects);
            hiredCount = 2;
        }

        // Update text value for Hired
        string statusText = hiredCount + "/2";
        SetOutputText(statusText);

        // Update text tag
        UpdateTextTag(hiredCount);
    }

    bool HasTag(GameObject obj, params string[] tags)
    {
        foreach (var tag in tags)
        {
            if (obj.CompareTag(tag)) return true;
        }
        return false;
    }

    void SetOutputText(string text)
    {
        outputText.text = text;
    }

    void ResetOtherTags(GameObject exception)
    {
        foreach (var obj in tagCheckObjects)
        {
            if (obj != exception)
            {
                obj.tag = "NotHired";
            }
        }
    }

    void ResetExcessHired(List<GameObject> hiredObjects)
    {
        for (int i = 2; i < hiredObjects.Count; i++)
        {
            hiredObjects[i].tag = "NotHired";
        }
    }

    void UpdateTextTag(int hiredCount)
    {
        switch (hiredCount)
        {
            case 0:
                SetOutputTextTag("NotHired");
                break;
            case 1:
                SetOutputTextTag("Hired");
                break;
            case 2:
                SetOutputTextTag("MaxHired");
                break;
        }
    }

    void SetOutputTextTag(string tag)
    {
        // Setting tag for the text component (in practice, just changing text since Text doesn't support tags)
        outputText.tag = tag;
    }
}