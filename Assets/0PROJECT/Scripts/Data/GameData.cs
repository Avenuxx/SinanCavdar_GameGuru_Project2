using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/GameData", order = 1)]

public class GameData : ScriptableObject
{
    [Header("Ints & Floats")]
    public int levelCount;
    [SerializeField] private float score;
    public float Score
    {
        get { return score; }
        set
        {
            if (value < 0) score = 0;
            else score = value;
        }
    }
    [SerializeField] private float totalMoney;
    public float TotalMoney
    {
        get { return totalMoney; }
        set
        {
            if (value < 0) totalMoney = 0;
            else totalMoney = value;
        }
    }
    public List<int> LevelStackCounts = new List<int>();


    [Button]
    public void ResetData()
    {
        levelCount = 0;
        Score = 0;
        TotalMoney = 0;
    }
}
