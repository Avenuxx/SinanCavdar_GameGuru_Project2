using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameState gameStateEnum;
    public GameData data;
    public StackGenerator stackGenerator;
    public GameObject player;
    public List<GameObject> stacksList = new List<GameObject>();
    public bool _isPlacedWrong;
    public int score;
    public int combo;
    public int stackCount;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stackGenerator = FindObjectOfType<StackGenerator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (gameStateEnum == GameState.Beginning)
            {
                EventManager.Broadcast(GameEvent.OnStart);
            }

            if (StackMovement.CurrentStack != null)
                EventManager.Broadcast(GameEvent.OnPlaceStack, StackMovement.CurrentStack);

            if (stackCount <= 0)
                return;

            if (_isPlacedWrong)
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
        data.levelCount++;
        stackCount = data.LevelStackCounts[data.levelCount];

        //SET FINISH OBJ POS
        GameObject finishObj = Instantiate(Resources.Load<GameObject>("Finish"));

        float prefabBoundZ = stackGenerator.stackPrefab.transform.localScale.z;
        float stackHeight = StackMovement.CurrentStack.transform.position.y;

        float finishPosZ = (data.LevelStackCounts[data.levelCount] * prefabBoundZ) - prefabBoundZ / 2;
        Vector3 finishPos = new Vector3(player.transform.position.x, -30, finishPosZ);

        Vector3 desiredPos = finishPos;
        desiredPos.y = stackHeight + 0.51f;

        finishObj.transform.position = finishPos;
        finishObj.transform.DOMove(desiredPos, 4f).SetEase(Ease.OutCubic);
    }

    void OnFinishLine()
    {
        gameStateEnum = GameState.FinishLine;
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnLose, OnLose);
        EventManager.AddHandler(GameEvent.OnFinishLine, OnFinishLine);
        EventManager.AddHandler(GameEvent.OnSetFinishLine, OnSetFinishLine);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnLose, OnLose);
        EventManager.RemoveHandler(GameEvent.OnFinishLine, OnFinishLine);
        EventManager.RemoveHandler(GameEvent.OnSetFinishLine, OnSetFinishLine);
    }
}
