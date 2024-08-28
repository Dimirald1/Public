using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    public GameObject unit;

    void Update()
    {
        if(unit.tag == "NoneUnit")
        {
            gameObject.tag = "Hired";
        }
    }
}
