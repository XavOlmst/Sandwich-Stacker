using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private GameObject enemySelector;
    [SerializeField] private float maxTimeQTEs;
    public PlayerAttacksDisplay playerAttackDisplay;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Animator playerAnimator;

    private int curEnemyIndex = 0;
    private bool isSelectPhase = false;
    private PlayerControls controls;
    private bool isChosing = false;
    private bool isPickingNextEnemy = false;
    private bool isPickingLastEnemy = false;


    public AudioSource music;
    public AudioSource SFX;
    public AudioSource louderSFX;
    public AudioClip win;
    public AudioClip enemyDeath;
    public AudioClip lose;
    public AudioClip menuMove;
    public AudioClip menuSelect;
  

    private void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        controls = new PlayerControls();

        controls.gameplay.MiddleLeft.started += ctx => { if (isSelectPhase) isChosing = true; };

        controls.gameplay.SideRight.started += ctx => isPickingNextEnemy = true;
        controls.gameplay.SideLeft.started += ctx => isPickingLastEnemy = true;

        playerAnimator.SetInteger("playerHealth", PlayerHealth.GetHealth());
    }

    private void Update()
    {
        playerAnimator.SetInteger("playerHealth", PlayerHealth.GetHealth());

        if (isPickingNextEnemy && isSelectPhase)
        {
            PickNextEnemy();
            isPickingNextEnemy = false;

        }

        if (isPickingLastEnemy && isSelectPhase)
        {
            PickLastEnemy();
            isPickingLastEnemy = false;
            
        }

        if (isChosing && isSelectPhase)
        {
            SelectSelectedEnemy();
            isChosing = false;
            playerAnimator.SetBool("selectedEnemy", true);
        }

        if (enemies.Length == 0)
        {
            playerAnimator.SetBool("allEnemiesDead", true);
            SFX.PlayOneShot(win);
        }
    }

    private void OnEnable()
    {
        controls.gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.gameplay.Disable();
    }

    void SelectSelectedEnemy()
    {
        ChangeSelectPhase();

        if (playerAttack.QTE.GetComponent<QTEBasicAttack>() != null)
        {
            louderSFX.PlayOneShot(menuSelect);
            QTEBasicAttack qte = playerAttack.QTE.GetComponent<QTEBasicAttack>();

            qte.gameObject.SetActive(true);
            qte.maxTime = maxTimeQTEs;
            qte.SetEnemy(enemies[curEnemyIndex]);

        }
        else if (playerAttack.QTE.GetComponent<QTETomatoAttack>() != null)
        {
            louderSFX.PlayOneShot(menuSelect);
            QTETomatoAttack qte = playerAttack.QTE.GetComponent<QTETomatoAttack>();

            qte.gameObject.SetActive(true);
            qte.SetEnemy(enemies[curEnemyIndex]);
        }
        else if (playerAttack.QTE.GetComponent<QTEBoneAttack>() != null)
        {
            louderSFX.PlayOneShot(menuSelect);
            QTEBoneAttack qte = playerAttack.QTE.GetComponent<QTEBoneAttack>();

            qte.gameObject.SetActive(true);
            qte.SetEnemy(enemies[curEnemyIndex]);
        }

    }

    public void CheckEnemyHealth()
    {
        foreach (GameObject enemy in enemies)
            if (enemy.GetComponent<EnemyScript>().GetHealth() <= 0)
            {
                louderSFX.PlayOneShot(enemyDeath);
                enemy.SetActive(false);
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                curEnemyIndex = 0;
            }
    }

    public void DamageAllEnemies(int damage)
    {
        foreach(GameObject enemy in enemies)
            enemy.GetComponent<EnemyScript>().LoseHealth(damage);
    }

    public void SetPlayerAttack(PlayerAttack newPlayerAttack)
    {
        playerAttack = newPlayerAttack;
        Debug.Log("Player Attack: " + playerAttack.playerAttack.damage);
    }
    public void ChangeSelectPhase()
    {
        StartCoroutine(Co_ChangeSelectPhase());
    }

    IEnumerator Co_ChangeSelectPhase()
    {
        yield return new WaitForSeconds(0.01f);

        isSelectPhase = !isSelectPhase;
        playerAnimator.SetBool("isSelectPhase", isSelectPhase);
        louderSFX.PlayOneShot(menuSelect);

        if (isSelectPhase)
        {
            enemySelector.SetActive(true);
            SetEnemySelectorPos();
        }
        else
            enemySelector.SetActive(false);
    }

    public void PickNextEnemy()
    {
        if (curEnemyIndex < enemies.Length - 1)
            curEnemyIndex++;
        else
            curEnemyIndex = 0;

        louderSFX.PlayOneShot(menuMove);
        SetEnemySelectorPos();
    }

    public void PickLastEnemy()
    {
        if(curEnemyIndex == 0)
            curEnemyIndex = enemies.Length - 1; 
        else
            curEnemyIndex--;

        louderSFX.PlayOneShot(menuMove);
        SetEnemySelectorPos();
    }

    public void SetEnemySelectorPos()
    {
        enemySelector.transform.position = new Vector3(enemies[curEnemyIndex].transform.position.x, enemies[curEnemyIndex].transform.position.y + 2, 
            enemies[curEnemyIndex].transform.position.z);
    }
    public void EnemyAttackPhase()
    {
        StartCoroutine(Co_AllEnemiesAttack());
        
    }

    IEnumerator Co_AllEnemiesAttack()
    {
        yield return new WaitForSeconds(1.0f);

        foreach(GameObject enemy in enemies)
        {
            EnemyScript enemyData = enemy.GetComponent<EnemyScript>();

            if (enemyData.GetHealth() <= 0)
                continue;



            if (enemyData.animator != null)
                enemyData.animator.SetTrigger("attack");
            else
                enemyData.AttackPlayer();

            yield return new WaitForSeconds(1.25f);

            if (PlayerHealth.GetHealth() <= 0)
            {
                playerAnimator.SetInteger("playerHealth", PlayerHealth.GetHealth());
                louderSFX.PlayOneShot(lose);
                break;
            }
        }

        yield return new WaitForSeconds(2.0f);

        if(enemies.Length != 0 && PlayerHealth.GetHealth() > 0)
            playerAttackDisplay.EnablePanel1();
    }
}
