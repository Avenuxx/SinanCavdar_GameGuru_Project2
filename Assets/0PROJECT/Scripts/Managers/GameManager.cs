using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState gameStateEnum;
    public List<GameObject> stacksList = new List<GameObject>();

    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (StackMovement.CurrentStack != null)
                EventManager.Broadcast(GameEvent.OnPlaceStack, StackMovement.CurrentStack);

            if (gameStateEnum == GameState.GameOver)
                return;

            EventManager.Broadcast(GameEvent.OnSpawnStack);
        }
    }
}
