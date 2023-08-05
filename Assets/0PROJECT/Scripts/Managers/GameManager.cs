using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameState gameStateEnum;
    public GameData data;
    public StackGenerator stackGenerator;

    [Serializable]
    public struct Objects
    {
        public GameObject player;
        public GameObject finishObj;
    }

    [Serializable]
    public struct Lists
    {
        public List<GameObject> stacksList;
    }

    [Serializable]
    public struct IntFloats
    {
        public int perfectStack;
        public int stackCount;
        public int perfectStackStreak;
    }

    [Serializable]
    public struct Bools
    {
        public bool _isPlacedWrong;
        public bool _canPlaceStack;
    }

    public Objects objects;
    public Lists lists;
    public IntFloats intFloats;
    public Bools bools;


    private void Awake()
    {
#if !UNITY_EDITOR
        SaveManager.LoadData(data);
#endif

        objects.player = GameObject.FindGameObjectWithTag("Player");
        stackGenerator = FindObjectOfType<StackGenerator>();

        InvokeRepeating(nameof(SaveData), 1f, 1f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (StackMovement.CurrentStack != null && gameStateEnum == GameState.Playing)
                EventManager.Broadcast(GameEvent.OnPlaceStack, StackMovement.CurrentStack);

            //IS PLAYER IS ON THE BEGINNING
            if (gameStateEnum == GameState.Beginning)
            {
                EventManager.Broadcast(GameEvent.OnStart);
            }

            //IS HAVE ENOUGH STACK
            if (!bools._canPlaceStack)
                return;

            //IS PLAYER PLACED THE STACK WRONG
            if (bools._isPlacedWrong)
                return;

            EventManager.Broadcast(GameEvent.OnSpawnStack);
        }
    }

    void OnStart()
    {
        gameStateEnum = GameState.Playing;
        ResetElements();
        EventManager.Broadcast(GameEvent.OnSetFinishLine);
    }

    void OnLose()
    {
        gameStateEnum = GameState.GameOver;
    }

    void OnWin()
    {
        gameStateEnum = GameState.FinishLine;
    }

    void OnNextLevel()
    {
        data.levelCount = Mathf.Min(data.levelCount + 1, 11);
        gameStateEnum = GameState.Beginning;
    }

    void ResetElements()
    {
        //RESET EVERY ELEMENT THAT CHANGE DURING PLAYING
        foreach (var stack in lists.stacksList)
        {
            Destroy(stack, 10f);
        }
        lists.stacksList.Clear();
        intFloats.perfectStack = 0;
        intFloats.perfectStackStreak = 0;
    }

    void SaveData()
    {
        SaveManager.SaveData(data);
    }



    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnLose, OnLose);
        EventManager.AddHandler(GameEvent.OnWin, OnWin);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnLose, OnLose);
        EventManager.RemoveHandler(GameEvent.OnWin, OnWin);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }
}
