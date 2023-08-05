using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/GameData", order = 1)]

public class GameData : ScriptableObject
{
    [Header("Ints & Floats")]
    public int levelCount;
    public int score;
    public List<int> LevelStackCounts = new List<int>();


    [Button]
    public void ResetData()
    {
        levelCount = 0;
        score = 0;
    }
}
