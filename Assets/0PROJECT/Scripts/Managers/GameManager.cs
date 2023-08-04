using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState gameStateEnum;
    public List<GameObject> stacksList = new List<GameObject>();
    public bool _isPlacedWrong;


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

            if (_isPlacedWrong)
                return;

            EventManager.Broadcast(GameEvent.OnSpawnStack);
        }
    }

    void OnStart()
    {
        gameStateEnum = GameState.Playing;
    }

    void OnLose()
    {
        gameStateEnum = GameState.GameOver;
    }



    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
    }
}
