using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerManager playerManager;
    GameManager manager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        manager = playerManager.manager;
        playerManager.structAnimation.playerAnim = GetComponentInChildren<Animator>();
    }

    void OnStart()
    {
        playerManager.structAnimation.playerAnim.SetBool("isStart", true);
    }

    void OnLose()
    {
        playerManager.structAnimation.playerAnim.SetTrigger("isFalling");
    }

    void OnFinishLine()
    {
        playerManager.structAnimation.playerAnim.SetTrigger("isLevelEnd");
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnLose, OnLose);
        EventManager.AddHandler(GameEvent.OnFinishLine, OnFinishLine);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnLose, OnLose);
        EventManager.RemoveHandler(GameEvent.OnFinishLine, OnFinishLine);
    }
}
