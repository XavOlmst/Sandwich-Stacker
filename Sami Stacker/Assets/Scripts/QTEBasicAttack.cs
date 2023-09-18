using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTEBasicAttack : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeDisplay;
    [SerializeField] private float percentIncrease;
    [SerializeField] private float decreaseRate;
    [SerializeField] private PlayerAttack attack;
    
    public float maxTime = 2.0f;

    private Image progressBar;
    private Animator playerAnimator;
    private bool isLeft;
    private bool hittingLeft;
    private bool hittingRight;
    private float timer;
    private PlayerControls controls;
    private bool perfectQTE;


    public AudioSource SFX;
    public AudioClip QTEsuccess;
    public AudioClip QTEfail;

    [HideInInspector] public GameObject enemy;

    private void Awake()
    {
        progressBar = GetComponent<Image>();
        playerAnimator = GetComponentInParent<Animator>();

        controls = new PlayerControls();

        controls.gameplay.MiddleLeft.started += ctx => hittingLeft = true;
        controls.gameplay.MiddleRight.started += ctx => hittingRight = true;
    }

    public void SetEnemy(GameObject enemy)
    {
        this.enemy = enemy;
        timer = 0;
        progressBar.fillAmount = 0;
    }

    void Update()
    {
        if (timer > maxTime)
        {
            perfectQTE = false;
            AttackEnemy();
            playerAnimator.SetTrigger("hammerAttack");
            gameObject.SetActive(false);
        }

        timeDisplay.text = ((int)(maxTime - timer) + 1).ToString();

        if (progressBar.fillAmount >= 1)
        {
            perfectQTE = true;
            AttackEnemy();
            playerAnimator.SetTrigger("hammerAttack");
            gameObject.SetActive(false);
        }

        if (isLeft && hittingLeft)
        {
            isLeft = false;
            hittingLeft = false;
            progressBar.fillAmount += percentIncrease / 100;
        }

        if (isLeft == false && hittingRight)
        {
            isLeft = true;
            hittingRight = false;
            progressBar.fillAmount += percentIncrease / 100;
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
        attack.AttackEnemy(enemy, perfectQTE, 1.25f);
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (progressBar.fillAmount > 0)
            progressBar.fillAmount -= decreaseRate / 100;
    }

    private void OnEnable()
    {
        controls.gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.gameplay.Disable();
    }
}
