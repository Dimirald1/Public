using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackModeOnOff : MonoBehaviour
{
    public Canvas baseMode;
    public Canvas attackMode;
    public GameObject plashka;
    public GameObject date;

    // Функция активации attackMode и деактивации baseMode
    public void ActivateAttackMode()
    {
        baseMode.gameObject.SetActive(false);
        attackMode.gameObject.SetActive(true);

        // Перемещаем plashkaOpen и plashkaDown в attackMode
        plashka.transform.SetParent(attackMode.transform, false);
        date.transform.SetParent(attackMode.transform, false);
    }

    // Функция активации baseMode и деактивации attackMode
    public void DeactivateAttackMode()
    {
        attackMode.gameObject.SetActive(false);
        baseMode.gameObject.SetActive(true);

        // Перемещаем plashkaOpen и plashkaDown в baseMode
        plashka.transform.SetParent(baseMode.transform, false);
        date.transform.SetParent(baseMode.transform, false);
    }
}
