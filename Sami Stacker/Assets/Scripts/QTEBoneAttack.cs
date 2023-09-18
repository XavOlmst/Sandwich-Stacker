using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEBoneAttack : MonoBehaviour
{
    [SerializeField] private int numKeys;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private GameObject[] displayObjects;
    [SerializeField] private Vector2 xMinMax;
    [SerializeField] private float yPos;
    [SerializeField] private float maxTime;

    public AudioSource SFX;
    public AudioClip QTEsuccess;
    public AudioClip QTEfail;
    public AudioClip QTEhit;
    public AudioClip QTEmiss;

    private PlayerControls controls;
    private Animator playerAnimator;
    private int[] keys;
    private GameObject enemy;
    private GameObject[] indicators;
    private int curKey = 0;
    private float timer = 0;
    private bool perfectQTE;

    private bool isMiddleLeft = false;
    private bool isMiddleRight = false;
    private bool isSideLeft = false;
    private bool isSideRight = false;

    public void Awake()
    {
        keys = new int[numKeys];
        controls = new PlayerControls();
        indicators = new GameObject[numKeys];
        playerAnimator = GetComponentInParent<Animator>();

        controls.gameplay.MiddleLeft.started += ctx => isMiddleLeft = true;
        controls.gameplay.MiddleRight.started += ctx => isMiddleRight = true;
        controls.gameplay.SideLeft.started += ctx => isSideLeft = true;
        controls.gameplay.SideRight.started += ctx => isSideRight = true;
    }

    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        curKey = 0;
        timer = 0;
        SetRandomInputs();
    }

    //Will add a timer later, just for testing rn

    void Update()
    {
        timer += Time.deltaTime;

        for (int i = 0; i < numKeys; i++)
        {
            if (i < curKey)
                indicators[i].SetActive(false);
            else
                indicators[i].SetActive(true);
        }

        if(isMiddleLeft)
        {
            isMiddleLeft = false;

            if (keys[curKey] == 0)
            {
                curKey++;
                SFX.PlayOneShot(QTEhit);
            }
            else
            {
                curKey = 0;
                SFX.PlayOneShot(QTEmiss);
            }
        }

        if(isMiddleRight)
        {
            isMiddleRight = false;

            if (keys[curKey] == 1)
            {
                curKey++;
                SFX.PlayOneShot(QTEhit);
            }
            else
            {
                curKey = 0;
                SFX.PlayOneShot(QTEmiss);
            }
        }

        if(isSideLeft)
        {
            isSideLeft = false;

            if (keys[curKey] == 2)
            {
                curKey++;
                SFX.PlayOneShot(QTEhit);
            }
            else
            {
                curKey = 0;
                SFX.PlayOneShot(QTEmiss);
            }
        }

        if (isSideRight)
        {
            isSideRight = false;

            if (keys[curKey] == 3)
            {
                curKey++;
                SFX.PlayOneShot(QTEhit);
            }
            else
            {
                curKey = 0;
                SFX.PlayOneShot(QTEmiss);
            }
        }

        if(timer > maxTime)
        {
            perfectQTE = false;
            AttackEnemy();
            playerAnimator.SetTrigger("boneAttack");
            gameObject.SetActive(false);
        }

        if(curKey == keys.Length)
        {
            perfectQTE = true;
            AttackEnemy();
            playerAnimator.SetTrigger("boneAttack");
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        controls.gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.gameplay.Disable();
        DestroyAllIndicators();
    }

    public void DestroyAllIndicators()
    {
        foreach (GameObject item in indicators)
        {
            Destroy(item);
        }
    }

    public void AttackEnemy()
    {
        if (perfectQTE == true)
        {
            SFX.PlayOneShot(QTEsuccess);
        }
        else
        {
            SFX.PlayOneShot(QTEfail);
        }
        attack.AttackEnemy(enemy, perfectQTE, 1.5f);
    }

    public void SetRandomInputs()
    {
        float segmentSize = (xMinMax.y - xMinMax.x) / numKeys;

        for (int i = 0; i < numKeys; i++)
        {
            int keyCode = Random.Range(0, 3);
            Debug.Log(keyCode);
            Vector2 pos = new Vector2(xMinMax.x + i * segmentSize / 2, yPos);

            keys[i] = keyCode;
            indicators[i] = Instantiate(displayObjects[keyCode], pos, Quaternion.identity, gameObject.transform);
            indicators[i].transform.localPosition = pos;
        }
    }
}
