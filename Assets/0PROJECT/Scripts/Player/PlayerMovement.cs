using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;
    GameManager manager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        manager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (IsGameOver()&& manager.gameStateEnum == GameState.Playing)
        {
            EventManager.Broadcast(GameEvent.OnLose);
            EventManager.Broadcast(GameEvent.OnPlaySound, "FallScream");
            return;
        }

        if (manager.gameStateEnum != GameState.Playing && manager.gameStateEnum != GameState.GameOver)
            return;

        MovementTarget();
    }

    private void MovementTarget()
    {
        //MOVEMENT TOWARD STACK
        if (playerManager.playerStateEnum == PlayerState.GoingStack)
        {
            //IS THERE ANY STACK TO GO
            if (manager.lists.stacksList.Count > playerManager.structMovement.goingStackIndex)
                playerManager.structMovement.target = manager.lists.stacksList[playerManager.structMovement.goingStackIndex].transform;

            //PLAYER NEED TO GO FORWARD
            else
            {
                playerManager.playerStateEnum = PlayerState.GoingForward;
                return;
            }

            Vector3 moveDirection = (playerManager.structMovement.target.position - transform.position).normalized;

            //GET THE DISTANCE BETWEEN STACK AND PLAYER
            float distanceToTarget = Vector3.Distance(transform.position, playerManager.structMovement.target.position);
            if (distanceToTarget > .7f)
                transform.position += moveDirection * playerManager.structMovement.moveSpeed * Time.deltaTime;

            //GET THE COUNT OF HOW MANY STACK DID PLAYER REACH
            else
                playerManager.structMovement.goingStackIndex++;
        }

        //MOVEMENT TOWARD FORWARD
        else if (playerManager.playerStateEnum == PlayerState.GoingForward)
        {
            //IS THERE ANY STACK TO GO
            if (playerManager.structMovement.goingStackIndex >= manager.lists.stacksList.Count)
                transform.position += Vector3.forward * playerManager.structMovement.moveSpeed * Time.deltaTime;

            else
                playerManager.playerStateEnum = PlayerState.GoingStack;
        }
    }

    private bool IsGameOver()
    {
        //CHECK FOR PLAYER Y POS
        if (transform.position.y < 0)
        {
            return true;
        }
        else
            return false;
    }

    void OnNextLevel()
    {
        playerManager.structMovement.goingStackIndex = 0;
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }
}
