using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    static string[] levelNames = { "Stage_1", "Stage_2", "Stage_3" };
    static int completedLevels = 0;
    static int maxLevels = 3;

    public static void CompletedLevel()
    {
        completedLevels++;
    }

    public static int GetMaxLevel() { return maxLevels; }
    public static int GetLevelsComplete() { return completedLevels; }
    public static void ResetLevels() { completedLevels = 0; }
    public static string GetCurrentLevel() { return levelNames[completedLevels]; }
}