using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    StackGenerator stackGenerator;
    private bool _isForward = true;

    public static StackMovement CurrentStack { get; private set; }
    public static StackMovement LastStack { get; private set; }

    private void OnEnable()
    {
        if (LastStack == null)
            LastStack = GameObject.Find("StartStack").GetComponent<StackMovement>();

        CurrentStack = this;
        stackGenerator = FindObjectOfType<StackGenerator>();
        GetComponent<Renderer>().material = stackGenerator.stackMaterials[stackGenerator.score];

        transform.localScale = new Vector3(LastStack.transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void Stop()
    {
        moveSpeed = 0;
        float diff = transform.position.x - LastStack.gameObject.transform.position.x;


        if (Mathf.Abs(diff) >= LastStack.transform.localScale.x)
        {
            LastStack = null;
            CurrentStack = null;
            Debug.Log("Game over");
            return;
        }

        float dir = diff > 0 ? 1 : -1;
        SplitCube(diff, dir);
        LastStack = GetComponent<StackMovement>();
    }

    private void SplitCube(float diff, float dir)
    {
        float newBoundsSize = LastStack.transform.localScale.x - Mathf.Abs(diff);
        float fallingStackSize = transform.localScale.x - newBoundsSize;

        float newBlockPosX = LastStack.transform.position.x + (diff / 2);
        transform.localScale = new Vector3(newBoundsSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newBlockPosX, transform.position.y, transform.position.z);

        float stackEdge = transform.position.x + (newBoundsSize / 2f * dir);
        float fallingStackXPos = stackEdge + fallingStackSize / 2f * dir;

        SpawnFallingStack(fallingStackXPos, fallingStackSize);
    }

    private void SpawnFallingStack(float fallingStackXPos, float fallingStackSize)
    {
        var newStack = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newStack.transform.localScale = new Vector3(fallingStackSize, transform.localScale.y, transform.localScale.z);
        newStack.transform.position = new Vector3(fallingStackXPos, transform.position.y, transform.position.z);

        newStack.AddComponent<Rigidbody>();
        newStack.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
        stackGenerator.score++;
        Destroy(newStack, 5f);
    }

    void Update()
    {
        var position = transform.position;
        var direction = _isForward ? 1 : -1;
        var move = moveSpeed * Time.deltaTime * direction;
        var limit = LastStack.transform.localScale.x + 1;

        position.x += move;

        if (position.x < -limit || position.x > limit)
        {
            position.x = Mathf.Clamp(position.x, -limit, limit);
            _isForward = !_isForward;
        }

        transform.position = position;
    }
}
