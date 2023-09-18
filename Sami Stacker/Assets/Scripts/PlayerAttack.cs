using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    public PlayerAttackScriptableObject playerAttack;
    public GameObject QTE;

    private void Awake()
    {
        QTE.SetActive(false);
    }

    public bool CheckIfCanAttack(GameObject enemy)
    {
        EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

        if (enemyScript.enemyData.isFlying && playerAttack.isRangedAttack == false)
            return false;

        if (enemyScript.enemyData.isArmored && playerAttack.canBreakArmor == false)
            return false;

        return true;
    }

    public void AttackEnemy(GameObject enemy, bool addBonusDamage, float delay)
    {
        StartCoroutine(Co_AttackEnemy(enemy, addBonusDamage, delay));
    }

    public IEnumerator Co_AttackEnemy(GameObject enemy, bool addBonusDamage, float delay)
    {
        yield return new WaitForSeconds(delay);

        EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

        if(playerAttack.canBreakArmor)
            enemyScript.RemoveArmor();

        if (playerAttack.isRangedAttack && enemyScript.GetFlyingState())
            enemyScript.ChangeFlyingState();

        if (addBonusDamage)
        {
            Debug.Log("Deal bonus damage: " + playerAttack.damage + playerAttack.bonusDamage);
            enemyScript.LoseHealth(playerAttack.damage + playerAttack.bonusDamage);
        }
        else
        {
            Debug.Log("Deal normal damage: " + playerAttack.damage);
            enemyScript.LoseHealth(playerAttack.damage);
        }

        //Using the combat manager to change the state of the game here
        CombatManager combatManager = GetComponentInParent<CombatManager>();

        if (addBonusDamage && playerAttack.isAOE)
        {
            Debug.Log("Attack is AOE and got QTE perfect");
            combatManager.DamageAllEnemies(1);
        }


        combatManager.CheckEnemyHealth();
        combatManager.EnemyAttackPhase();

        QTE.SetActive(false);
    }
    
}
