using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player Attack")]
public class PlayerAttackScriptableObject : ScriptableObject
{
    public bool canBreakArmor;
    public bool isRangedAttack;
    public bool isAOE;
    public int damage;
    public int bonusDamage = 2;

}
