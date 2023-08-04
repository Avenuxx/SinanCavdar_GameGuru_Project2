using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMovement : MonoBehaviour
{
    public float moveSpeed = 1f;

    public static StackMovement CurrentStack { get; private set; }
    public static StackMovement LastStack { get; private set; }

    private void OnEnable()
    {
        if (LastStack == null)
            LastStack = GameObject.Find("StartStack").GetComponent<StackMovement>();

        CurrentStack = this;
    }

    public void Stop()
    {
        moveSpeed = 0;
        float diff = transform.position.x - LastStack.gameObject.transform.position.x;
        SplitCube(diff);
    }

    private void SplitCube(float diff)
    {
        float newBoundsSize = LastStack.transform.position.x - Mathf.Abs(diff);
        float fallingStackSize = transform.localScale.x - newBoundsSize;

        float newBlockPosX = LastStack.transform.position.x + (diff / 2);
        transform.localScale = new Vector3(newBoundsSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newBlockPosX, transform.position.y, transform.position.z);

        float stackEdge = transform.position.z + newBoundsSize / 2f;
        float fallingStackXPos = stackEdge + fallingStackSize / 2f;

        SpawnFallingStack(fallingStackXPos, fallingStackSize);
    }

    private void SpawnFallingStack(float fallingStackXPos, float fallingStackSize)
    {
        var newStack = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newStack.transform.localScale = new Vector3(fallingStackSize, transform.localScale.y, transform.localScale.z);
        newStack.transform.position = new Vector3(fallingStackXPos, transform.position.y, transform.position.z);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * moveSpeed;
    }
}
