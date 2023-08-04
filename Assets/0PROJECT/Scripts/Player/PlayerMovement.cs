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
        if (manager.gameStateEnum != GameState.Playing && manager.gameStateEnum != GameState.GameOver)
            return;

        if (playerStateEnum == PlayerState.GoingCube)
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
        else if (playerStateEnum == PlayerState.GoingForward)
        {
            if (goingStackIndex >= manager.stacksList.Count)
                transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

            else
                playerStateEnum = PlayerState.GoingCube;
        }
    }

    private void GameOverCheck()
    {
        if (transform.position.y<0)
        {
            
        }
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
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
    }
}
