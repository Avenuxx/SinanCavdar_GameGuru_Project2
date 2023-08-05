using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackMovement : MonoBehaviour
{
    GameManager manager;
    StackGenerator stackGenerator;

    public float moveSpeed = 1f;
    private bool _isForward = true;
    public float perfectThreshold = 0.15f;

    public static StackMovement CurrentStack { get; private set; }
    public static StackMovement LastStack { get; private set; }

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        stackGenerator = FindObjectOfType<StackGenerator>();
    }

    private void Start()
    {
        //GET THE LAST STACK
        if (LastStack == null)
        {
            LastStack = stackGenerator.startStack.GetComponent<StackMovement>();
        }

        //SET MATERIAL OF NEW STACK
        CurrentStack = this;
        var renderer = GetComponent<Renderer>();
        renderer.material = stackGenerator.stackMaterials[manager.intFloats.stackCount % 12];

        //SET SCALE OF NEW STACK
        var localScale = transform.localScale;
        localScale.x = LastStack.transform.localScale.x;
        transform.localScale = localScale;
    }

    void Update()
    {
        if (manager.bools._isPlacedWrong)
            return;

        //LEFT-RIGHT MOVEMENT OF CURRENT STACK
        var direction = _isForward ? 1 : -1;
        var move = moveSpeed * Time.deltaTime * direction;
        var limit = LastStack.transform.localScale.x + 1;

        Vector3 newPosition = transform.position + Vector3.right * move;
        newPosition.x = Mathf.Clamp(newPosition.x, -limit, limit);

        //CHANGE OF DIRECTION
        if (Mathf.Abs(newPosition.x) >= limit)
        {
            _isForward = !_isForward;
        }

        transform.position = newPosition;
    }

    public void OnPlaceStack(object value)
    {
        if ((StackMovement)value != this)
            return;

        moveSpeed = 0;

        var diff = transform.position.x - LastStack.gameObject.transform.position.x;

        CheckForGameOverOrPerfect(diff);
    }

    private void CheckForGameOverOrPerfect(float diff)
    {
        var stackScaleX = LastStack.transform.localScale.x;

        if (!manager.bools._canPlaceStack)
            return;

        if (manager.intFloats.stackCount == 0)
            manager.bools._canPlaceStack = false;

        //ON WRONG PLACED OF STACK
        if (Mathf.Abs(diff) >= stackScaleX)
        {
            LastStack = null;
            CurrentStack = null;
            gameObject.AddComponent<Rigidbody>();
            manager.bools._isPlacedWrong = true;
            EventManager.Broadcast(GameEvent.OnPlaySound, "FailStack");
            return;
        }


        //ON PERFECT PLACED OF STACK
        if (Mathf.Abs(diff) <= perfectThreshold)
        {
            var perfectPosition = new Vector3(LastStack.transform.position.x, transform.position.y, transform.position.z);
            transform.position = perfectPosition;
            LastStack = GetComponent<StackMovement>();

            //SCORE INCREASE
            manager.data.Score += 3;
            manager.lists.stacksList.Add(gameObject);
            manager.intFloats.perfectStack++;

            //PERFECT STREAK CALCULATION
            if (manager.intFloats.perfectStack > manager.intFloats.perfectStackStreak)
                manager.intFloats.perfectStackStreak = manager.intFloats.perfectStack;

            transform.LeanScale(transform.localScale * 1.15f, 0.7f).setEasePunch();

            EventManager.Broadcast(GameEvent.OnPerfectPlaceStack, gameObject);

            //PLAY AUDIO AND CAMERA SHAKE
            var pitchAmount = 1f + ((manager.intFloats.perfectStack + 1f) / 10f);
            EventManager.Broadcast(GameEvent.OnPlaySoundPitch, "Note", pitchAmount);
            CinemachineShake.Instance.ShakeCamera(.7f, .3f);
            return;
        }

        //ON NORMAL PLACED OF STACK
        manager.intFloats.perfectStack = 0;
        manager.lists.stacksList.Add(gameObject);
        var dir = diff > 0 ? 1 : -1;
        SplitCube(diff, dir);
        LastStack = GetComponent<StackMovement>();
        EventManager.Broadcast(GameEvent.OnPlaySound, "Break");
    }

    private void SplitCube(float diff, float dir)
    {
        //SET NEW SIZE OF THE STACK
        var newBoundsSize = LastStack.transform.localScale.x - Mathf.Abs(diff);
        var fallingStackSize = transform.localScale.x - newBoundsSize;

        //SET NEW POS OF THE STACK
        var newBlockPosX = LastStack.transform.position.x + (diff / 2);
        transform.localScale = new Vector3(newBoundsSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newBlockPosX, transform.position.y, transform.position.z);

        var stackEdge = transform.position.x + (newBoundsSize / 2f * dir);
        var fallingStackXPos = stackEdge + fallingStackSize / 2f * dir;

        SpawnFallingStack(fallingStackXPos, fallingStackSize);
    }

    private void SpawnFallingStack(float fallingStackXPos, float fallingStackSize)
    {
        var newStack = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //SET SIZE OF THE FALLING STACK
        newStack.transform.localScale = new Vector3(fallingStackSize, transform.localScale.y, transform.localScale.z);
        newStack.transform.position = new Vector3(fallingStackXPos, transform.position.y, transform.position.z);

        //SET POS OF THE FALLING STACK
        var newStackRigidbody = newStack.AddComponent<Rigidbody>();
        newStackRigidbody.useGravity = true;

        var newStackRenderer = newStack.GetComponent<Renderer>();
        var thisStackRenderer = GetComponent<Renderer>();
        newStackRenderer.material = thisStackRenderer.material;

        manager.data.Score++;

        Destroy(newStack, 5f);
    }

    private void OnNextLevel()
    {
        LastStack = stackGenerator.startStack.GetComponent<StackMovement>();
    }



    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlaceStack, OnPlaceStack);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlaceStack, OnPlaceStack);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }
}
