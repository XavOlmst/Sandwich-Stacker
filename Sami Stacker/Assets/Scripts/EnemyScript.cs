using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private int curHealth;
    private bool isCurrentlyArmored;
    private bool isCurrentlyFlying;
    [SerializeField] private float hamFallOffset;



    public AudioSource source;
    public AudioSource hamSource;
    public AudioClip enemySpecific;
    public AudioClip enemySpecific2;
    public AudioClip damage;
    public AudioClip atkSound;

    private bool hamFell = false;
    private Vector3 secondaryPos;
    private Animator playerAnimator;

    [HideInInspector] public Animator animator;

    void Start()
    {
        curHealth = enemyData.maxHealth;
        isCurrentlyArmored = enemyData.isArmored;
        isCurrentlyFlying = enemyData.isFlying;
        animator = GetComponent<Animator>();

        if(animator != null)
            animator.SetInteger("health", curHealth);
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        secondaryPos = new Vector3(transform.localPosition.x, transform.localPosition.y - hamFallOffset, transform.localPosition.z);
        Debug.Log(secondaryPos);
    }

    private void FixedUpdate()
    {
        if(hamFell)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, secondaryPos, hamFallOffset / 60);
        }

        if (Vector3.Distance(transform.localPosition, secondaryPos) < 0.1f && hamFell)
        {
            Debug.Log("Finished the fall");
            transform.localPosition = secondaryPos;
            hamFell = false;
        }
    }

    public int GetHealth() { return curHealth; }
    public bool GetFlyingState() { return isCurrentlyFlying; }

    public void AttackPlayer()
    {
        playerAnimator.SetTrigger("enemyAttack");
        PlayerHealth.LoseHealth(enemyData.damage);
        source.PlayOneShot(atkSound);
    }

    public void LoseHealth(int healthLost) 
    {
        if (isCurrentlyArmored == false && isCurrentlyFlying == false)
        {
            curHealth -= healthLost;
            source.PlayOneShot(damage);
            animator.SetTrigger("takeDamage");
        }

        if (isCurrentlyArmored)
            source.PlayOneShot(enemySpecific2);
    }
    
    public void RemoveArmor()
    {
        if (isCurrentlyArmored)
        {
            isCurrentlyArmored = false;
            source.PlayOneShot(enemySpecific);
        }
    }

    public void ChangeFlyingState()
    {
        animator.SetBool("isFlying", isCurrentlyFlying);

        isCurrentlyFlying = !isCurrentlyFlying;
        animator.SetTrigger("changeFlyState");

        if (isCurrentlyFlying == false)
        {
            Debug.Log("Changed the flying state");
            source.PlayOneShot(enemySpecific, 0.3f);
            hamFell = true;
        }
    }
}
