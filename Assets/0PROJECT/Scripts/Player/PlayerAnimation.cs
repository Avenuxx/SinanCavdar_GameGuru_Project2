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

    void OnWin()
    {
        playerManager.structAnimation.playerAnim.SetTrigger("isLevelEnd");
    }

    void OnNextLevel()
    {
        playerManager.structAnimation.playerAnim.SetBool("isStart", false);
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnLose, OnLose);
        EventManager.AddHandler(GameEvent.OnWin, OnWin);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnLose, OnLose);
        EventManager.RemoveHandler(GameEvent.OnWin, OnWin);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }
}
