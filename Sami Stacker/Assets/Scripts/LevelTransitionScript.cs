using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionScript : MonoBehaviour
{
    [SerializeField] private GameObject[] targetPoints;
    [SerializeField] private GameObject displayObject;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject[] attackInfoObjects;

    private Vector2 targetPoint;
    private Animator objectAnimator;

    private void Awake()
    {
        objectAnimator = displayObject.GetComponent<Animator>();
        objectAnimator.SetBool("isWalking", false);

        if (LevelManager.GetLevelsComplete() == LevelManager.GetMaxLevel())
        {
            SceneManager.LoadScene("End_Screen_Win");
            return;
        }

        if (LevelManager.GetLevelsComplete() != 0)
        {
            displayObject.transform.position = targetPoints[LevelManager.GetLevelsComplete() - 1].transform.position;
            Debug.Log(displayObject.transform.position);
            objectAnimator.SetBool("isWalking", true);
        }
        else
            displayObject.transform.position = targetPoints[0].transform.position;

        foreach (GameObject obj in attackInfoObjects)
            obj.SetActive(false);

        targetPoint = targetPoints[LevelManager.GetLevelsComplete()].transform.position;
        Debug.Log(targetPoint);
    }

    void Update()
    {
        if(Vector2.Distance(displayObject.transform.position, targetPoint) < 0.01f)
        {
            objectAnimator.SetBool("isWalking", false);

            for (int i = 0; i < attackInfoObjects.Length; i++)
            {
                if (i != LevelManager.GetLevelsComplete())
                    attackInfoObjects[i].SetActive(false);
                else
                    attackInfoObjects[i].SetActive(true);
            }

            if (Input.anyKeyDown)
                SceneManager.LoadScene(LevelManager.GetCurrentLevel());

            return;
        }

        displayObject.transform.position = Vector2.MoveTowards(displayObject.transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }
}
