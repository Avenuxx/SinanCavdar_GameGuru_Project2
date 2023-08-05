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
        if (IsGameOver())
        {
            EventManager.Broadcast(GameEvent.OnLose);
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
            if (manager.stacksList.Count > playerManager.structMovement.goingStackIndex)
                playerManager.structMovement.target = manager.stacksList[playerManager.structMovement.goingStackIndex].transform;

            else
            {
                playerManager.playerStateEnum = PlayerState.GoingForward;
                return;
            }

            Vector3 moveDirection = (playerManager.structMovement.target.position - transform.position).normalized;

            float distanceToTarget = Vector3.Distance(transform.position, playerManager.structMovement.target.position);
            if (distanceToTarget > .7f)
                transform.position += moveDirection * playerManager.structMovement.moveSpeed * Time.deltaTime;

            else
                playerManager.structMovement.goingStackIndex++;
        }

        //MOVEMENT TOWARD FORWARD
        else if (playerManager.playerStateEnum == PlayerState.GoingForward)
        {
            if (playerManager.structMovement.goingStackIndex >= manager.stacksList.Count)
                transform.position += Vector3.forward * playerManager.structMovement.moveSpeed * Time.deltaTime;

            else
                playerManager.playerStateEnum = PlayerState.GoingStack;
        }
    }

    private bool IsGameOver()
    {
        if (transform.position.y < 0)
        {
            return true;
        }
        else
            return false;
    }

   
}
