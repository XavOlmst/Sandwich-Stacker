using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacksDisplay : MonoBehaviour
{
    [SerializeField] private GameObject attackPanel1;
    [SerializeField] private GameObject attackPanel2;

    public void DisablePanel1()
    {
        attackPanel1.SetActive(false);
    }

    public void EnablePanel1()
    {
        attackPanel1.SetActive(true);
    }

    public void DisablePanel2()
    {
        attackPanel2.SetActive(false);
    }

    public void EnablePanel2()
    {
        attackPanel2.SetActive(true);
    }
}
