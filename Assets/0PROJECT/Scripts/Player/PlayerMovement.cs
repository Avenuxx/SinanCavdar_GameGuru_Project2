using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameManager manager;
    Animator playerAnim;
    public PlayerState playerStateEnum;
    public Transform target;
    public float moveSpeed = 1.0f;
    public int goingStackIndex;


    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        playerAnim = GetComponentInChildren<Animator>();
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
        if (playerStateEnum == PlayerState.GoingStack)
        {
            if (manager.stacksList.Count > goingStackIndex)
                target = manager.stacksList[goingStackIndex].transform;

            else
                playerStateEnum = PlayerState.GoingForward;

            Vector3 moveDirection = (target.position - transform.position).normalized;

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget > .7f)
                transform.position += moveDirection * moveSpeed * Time.deltaTime;

            else
                goingStackIndex++;
        }
        //MOVEMENT TOWARD FORWARD
        else if (playerStateEnum == PlayerState.GoingForward)
        {
            if (goingStackIndex >= manager.stacksList.Count)
                transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

            else
                playerStateEnum = PlayerState.GoingStack;
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

    void OnStart()
    {
        playerAnim.SetBool("isStart",true);
    }

    void OnLose()
    {
        playerAnim.SetTrigger("isFalling");
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnLose, OnLose);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnLose, OnLose);
    }
}
