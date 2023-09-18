using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerHealth
{
    private static int maxHealth = 50;
    private static int curHealth = maxHealth;


    
    public static int GetHealth() { return curHealth; }
    public static void LoseHealth(int healthLost) { curHealth -= healthLost; }
    public static void SetHealth(int health) { curHealth = health; }
    public static void GainHealth(int healthGained) 
    {
        Debug.Log("Healing Player");

        if (curHealth + healthGained < maxHealth)
            curHealth += healthGained;
        else
            FullHeal();
    }
    public static void FullHeal() { curHealth = maxHealth; }

    public static int GetMaxHealth() { return maxHealth; }
    public static void IncreaseMaxHealth(int healthIncreased) { maxHealth += healthIncreased; }
}
