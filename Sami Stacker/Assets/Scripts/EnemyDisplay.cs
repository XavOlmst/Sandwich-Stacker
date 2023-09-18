using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDisplay : MonoBehaviour
{
    private EnemyScript enemyScript;
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] Image healthBar;

    private void Awake()
    {
        enemyScript = GetComponent<EnemyScript>();
        textDisplay = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Update()
    {
        textDisplay.text = enemyScript.GetHealth() + "/" + enemyScript.enemyData.maxHealth;
        healthBar.fillAmount = (float) enemyScript.GetHealth() / enemyScript.enemyData.maxHealth;
    }
}
