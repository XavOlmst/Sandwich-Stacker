using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;

    //Temporary
    private void Awake()
    {
        //PlayerHealth.FullHeal();
    }

    void Update()
    {
        healthText.text = PlayerHealth.GetHealth().ToString() + "/" + PlayerHealth.GetMaxHealth().ToString();

        healthBar.fillAmount = (float) PlayerHealth.GetHealth() / PlayerHealth.GetMaxHealth();
    }
}
