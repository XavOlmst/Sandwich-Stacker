using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Restart()
    {
        PlayerHealth.FullHeal();
        LevelManager.ResetLevels();
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NavigationScene()
    {
        SceneManager.LoadScene("Stage_Transition");
    }

    public void Level1()
    {
        SceneManager.LoadScene("Stage_1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Stage_2");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Stage_3");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
