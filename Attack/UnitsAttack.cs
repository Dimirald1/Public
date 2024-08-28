using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitsAttack : MonoBehaviour
{
    public short maxHealth;
    public short hp;
    public float defense;
    public float attackSpeedToUnitsMonster;
    public float damageToMonsters;
    public float damageToUnits;

    private bool isAttacking = false;
    private bool Attacked = false;


    public IEnumerator AttackTarget(GameObject target)
    {
        isAttacking = true;

        // Атакуем цель каждые attackSpeedToUnitsMonster секунд
        while (isAttacking)
        {
            yield return new WaitForSeconds(attackSpeedToUnitsMonster);

            // Получаем компоненты UnitsAttack у цели
            UnitsAttack targetAttack = target.GetComponent<UnitsAttack>();

            if (targetAttack != null)
            {
                // Вычисляем урон, наносящийся цели
                float damage = Mathf.Max(0, damageToUnits - targetAttack.defense);
                targetAttack.hp -= (short)damage;

                // Проверяем, не умер ли объект
                if (targetAttack.hp <= 0)
                {
                    isAttacking = false;
                    
                }
            }
        }
    }
}
