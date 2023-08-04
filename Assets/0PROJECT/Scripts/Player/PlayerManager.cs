using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameManager manager;
    public GameData data;
    public PlayerMovement playerMovement;
    public PlayerAnimation playerAnimation;
    public PlayerTrigger playerTrigger;
    public PlayerState playerStateEnum;

    [Serializable]
    public struct StructMovement
    {
        public Transform target;
        public float moveSpeed;
        public int goingStackIndex;
    }

    [Serializable]
    public struct StructAnimation
    {
        public Animator playerAnim;
    }

    [Serializable]
    public struct StructTrigger
    {

    }

    public StructMovement structMovement;
    public StructAnimation structAnimation;
    public StructTrigger structTrigger;

    private void Awake()
    {
        PlayerDefinitons();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayerDefinitons()
    {
        manager = FindObjectOfType<GameManager>();
        data = manager.data;
        playerMovement = GetComponent<PlayerMovement>();
        playerTrigger = GetComponent<PlayerTrigger>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }
}
