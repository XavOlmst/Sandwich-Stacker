using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTETomatoAttack : MonoBehaviour
{
    [SerializeField] private GameObject spawnedItem;
    [SerializeField] private float itemRadius;
    [SerializeField] private int numItems;
    [SerializeField] private Vector2 xMinMax;
    [SerializeField] private GameObject movingBar;
    [SerializeField] private float barMoveDelay;
    [SerializeField] private float barSpeed;
    [SerializeField] private float barOffset = -0.1f;
    [SerializeField] private PlayerAttack attack;

    public AudioSource SFX;
    public AudioClip QTEsuccess;
    public AudioClip QTEfail;
    public AudioClip QTEhit;
    public AudioClip QTEmiss;

    [HideInInspector] public bool completedQTE = true;

    private float timer;
    private GameObject enemy;
    private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponentInParent<Animator>();
    }

    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        timer = 0;
        completedQTE = true;
        movingBar.transform.localPosition = new Vector2(xMinMax.x + barOffset, 0);
        SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= barMoveDelay)
        {
            movingBar.SetActive(true);
            movingBar.GetComponent<Rigidbody2D>().velocity = new Vector2(barSpeed, 0);
        }

        if (movingBar.transform.localPosition.x > xMinMax.y - barOffset)
        {
            if(completedQTE)
                completedQTE = ItemsDontExist();

            movingBar.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ItemsDontExist();
            playerAnimator.SetTrigger("tomatoAttack");
            AttackEnemy();
            gameObject.SetActive(false);
        }
    }

    public void AttackEnemy()
    {
        attack.AttackEnemy(enemy, completedQTE, 1.0f);
    }

    public bool ItemsDontExist()
    {
        if (GameObject.FindGameObjectsWithTag("Tomato Attack QTE Object").Length > 0)
        {
            DestroyAllObjects();
            return false;
        }

        return true;
    }

    public void DestroyAllObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Tomato Attack QTE Object");

        foreach(GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    public void SpawnObjects()
    {
        float segmentSize = (xMinMax.y - xMinMax.x) / numItems;

        for(int i = 0; i < numItems; i++)
        {
            Vector2 pos = new Vector2(Random.Range(xMinMax.x + segmentSize * i + itemRadius, xMinMax.y - segmentSize * (numItems - (i + 1)) - itemRadius), 0);
            GameObject obj = Instantiate(spawnedItem, pos, Quaternion.identity, gameObject.transform);
            obj.transform.localPosition = pos;
        }
    }
}
