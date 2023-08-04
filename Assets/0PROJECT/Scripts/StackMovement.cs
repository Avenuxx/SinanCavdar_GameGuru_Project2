using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMovement : MonoBehaviour
{
    GameManager manager;
    StackGenerator stackGenerator;

    public float moveSpeed = 1f;
    private bool _isForward = true;

    public static StackMovement CurrentStack { get; private set; }
    public static StackMovement LastStack { get; private set; }

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        if (LastStack == null)
        {
            LastStack = GameObject.Find("StartStack").GetComponent<StackMovement>();
        }

        CurrentStack = this;
        stackGenerator = FindObjectOfType<StackGenerator>();
        var renderer = GetComponent<Renderer>();
        renderer.material = stackGenerator.stackMaterials[stackGenerator.score % 12];

        var localScale = transform.localScale;
        localScale.x = LastStack.transform.localScale.x;
        transform.localScale = localScale;
    }

    void Update()
    {
        if (manager._isPlacedWrong)
            return;

        var direction = _isForward ? 1 : -1;
        var move = moveSpeed * Time.deltaTime * direction;
        var limit = LastStack.transform.localScale.x + 1;

        Vector3 newPosition = transform.position + Vector3.right * move;
        newPosition.x = Mathf.Clamp(newPosition.x, -limit, limit);

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
        if (Mathf.Abs(diff) >= stackScaleX)
        {
            LastStack = null;
            CurrentStack = null;
            gameObject.AddComponent<Rigidbody>();
            manager._isPlacedWrong=true;
            return;
        }

        var perfectThreshold = 0.2f;
        if (Mathf.Abs(diff) <= perfectThreshold)
        {
            var perfectPosition = new Vector3(LastStack.transform.position.x, transform.position.y, transform.position.z);
            transform.position = perfectPosition;
            LastStack = GetComponent<StackMovement>();
            stackGenerator.score++;
            manager.stacksList.Add(gameObject);
            return;
        }

        manager.stacksList.Add(gameObject);
        var dir = diff > 0 ? 1 : -1;
        SplitCube(diff, dir);
        LastStack = GetComponent<StackMovement>();
    }

    private void SplitCube(float diff, float dir)
    {
        var newBoundsSize = LastStack.transform.localScale.x - Mathf.Abs(diff);
        var fallingStackSize = transform.localScale.x - newBoundsSize;

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
        newStack.transform.localScale = new Vector3(fallingStackSize, transform.localScale.y, transform.localScale.z);
        newStack.transform.position = new Vector3(fallingStackXPos, transform.position.y, transform.position.z);

        var newStackRigidbody = newStack.AddComponent<Rigidbody>();
        newStackRigidbody.useGravity = true;

        var newStackRenderer = newStack.GetComponent<Renderer>();
        var thisStackRenderer = GetComponent<Renderer>();
        newStackRenderer.material = thisStackRenderer.material;

        stackGenerator.score++;

        Destroy(newStack, 5f);
    }



    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlaceStack, OnPlaceStack);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlaceStack, OnPlaceStack);
    }
}
