using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimationScript : MonoBehaviour
{
    public void ToTransitionScene()
    {
        LevelManager.CompletedLevel();
        SceneManager.LoadScene("Stage_Transition");
    }

    public void Heal()
    {
        PlayerHealth.GainHealth(15);
    }

    public void GameLost()
    {
        SceneManager.LoadScene("End_Screen_Lose");
    }
}
