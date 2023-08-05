using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        if (IsGameOver() && manager.gameStateEnum == GameState.Playing)
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
        switch (playerManager.playerStateEnum)
        {
            case PlayerState.GoingStack:
                #region SetPlayerTarget
                //IS THERE ANY STACK TO GO
                if (manager.lists.stacksList.Count > playerManager.structMovement.goingStackIndex)
                    playerManager.structMovement.target = manager.lists.stacksList[playerManager.structMovement.goingStackIndex].transform;

                else
                {
                    playerManager.playerStateEnum = PlayerState.GoingForward;
                    return;
                }
                #endregion

                #region MoveToStack
                Vector3 moveDirection = (playerManager.structMovement.target.position - transform.position).normalized;

                //GET THE DISTANCE BETWEEN STACK AND PLAYER
                float distanceToTarget = Vector3.Distance(transform.position, playerManager.structMovement.target.position);
                if (distanceToTarget > .7f)
                    transform.position += moveDirection * playerManager.structMovement.moveSpeed * Time.deltaTime;

                else
                    playerManager.structMovement.goingStackIndex++;
                #endregion
                break;

            case PlayerState.GoingForward:
                #region MoveForward
                //IS THERE ANY STACK TO GO
                if (playerManager.structMovement.goingStackIndex >= manager.lists.stacksList.Count)
                    transform.position += Vector3.forward * playerManager.structMovement.moveSpeed * Time.deltaTime;

                else
                    playerManager.playerStateEnum = PlayerState.GoingStack;
                #endregion
                break;
        }
    }

    private bool IsGameOver()
    {
        //CHECK FOR PLAYER Y POS
        bool gameState;
        gameState = transform.position.y < 0 ? true : false;
        return gameState;
    }

    void OnNextLevel()
    {
        playerManager.structMovement.goingStackIndex = 0;
        transform.DOJump(new Vector3(0, transform.position.y, transform.position.z + 1.5f), 1f, 1, .5f);
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
