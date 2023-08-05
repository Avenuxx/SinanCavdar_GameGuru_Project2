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

            if (gameStateEnum == GameState.Beginning)
            {
                EventManager.Broadcast(GameEvent.OnStart);
            }

            if (!bools._canPlaceStack)
                return;

            if (bools._isPlacedWrong)
                return;

            EventManager.Broadcast(GameEvent.OnSpawnStack);
        }
    }

    void OnStart()
    {
        gameStateEnum = GameState.Playing;
        EventManager.Broadcast(GameEvent.OnSetFinishLine);
    }

    void OnLose()
    {
        gameStateEnum = GameState.GameOver;
    }

    void OnSetFinishLine()
    {
        intFloats.stackCount = data.LevelStackCounts[data.levelCount % 12];
        bools._canPlaceStack = true;

        //SET FINISH OBJ POS
        GameObject finishObj = Instantiate(Resources.Load<GameObject>("Finish"));

        float prefabBoundZ = stackGenerator.stackPrefab.transform.localScale.z;
        float stackHeight = StackMovement.CurrentStack.transform.position.y;

        float finishPosZ = stackGenerator.startStack.transform.position.z + (((data.LevelStackCounts[data.levelCount % 12] + 1) * prefabBoundZ) - prefabBoundZ / 5);
        Debug.Log(finishPosZ);
        Vector3 finishPos = new Vector3(objects.player.transform.position.x, -30, finishPosZ);

        Vector3 desiredPos = finishPos;
        desiredPos.y = stackHeight + 0.51f;

        finishObj.transform.position = finishPos;
        finishObj.transform.DOMove(desiredPos, 4f).SetEase(Ease.OutCubic);

        this.objects.finishObj = finishObj;

        //SET FINISH CAM POS
        Vector3 camDesiredPos = desiredPos;
        camDesiredPos.y = 5;
        CameraManager.Instance.CMFinishLine.transform.parent.position = camDesiredPos;
    }

    void OnWin()
    {
        gameStateEnum = GameState.FinishLine;
    }

    void OnNextLevel()
    {
        data.levelCount++;
        gameStateEnum = GameState.Beginning;
        ResetElements();
    }

    void ResetElements()
    {
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
        EventManager.AddHandler(GameEvent.OnSetFinishLine, OnSetFinishLine);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnLose, OnLose);
        EventManager.RemoveHandler(GameEvent.OnWin, OnWin);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnSetFinishLine, OnSetFinishLine);
    }
}
