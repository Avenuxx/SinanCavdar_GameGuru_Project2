using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackGenerator : InstanceManager<StackGenerator>
{
    GameManager manager;
    StackMovement stackPrefab;

    [Header("Materials")]
    public Material[] stackMaterials;

    [Space(10)]
    [Header("GameObjects")]
    public GameObject startStack;

    [Space(10)]
    [Header("Transforms")]
    public Transform stackParent;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        startStack = GameObject.Find("StartStack");
    }

    public void OnSpawnStack()
    {
        manager.intFloats.stackCount--;

        var stack = Instantiate(stackPrefab, stackParent);

        //SET NEW STACK POSITION
        if (StackMovement.LastStack != null && StackMovement.LastStack.gameObject != startStack)
        {
            int xPos = manager.data.Score % 2 == 0 ? -4 : 4;
            var stackPosition = new Vector3(xPos, transform.position.y, StackMovement.LastStack.transform.position.z + stackPrefab.transform.localScale.z);
            stack.transform.position = stackPosition;
        }
        //SET FIRST GENERATOR POSITION
        else
        {
            stack.transform.position = new Vector3(-4, startStack.transform.position.y, startStack.transform.position.z + stackPrefab.transform.localScale.z);
        }

        EventManager.Broadcast(GameEvent.OnSpawnCollectable, stack.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, stackPrefab.transform.localScale);
    }

    private void OnNextLevel()
    {
        //SET GENERATOR POSITION
        var desiredPos = new Vector3(0, stackPrefab.transform.position.y, manager.objects.finishObj.transform.position.z + 2);
        startStack.transform.position = desiredPos;
    }



    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSpawnStack, OnSpawnStack);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSpawnStack, OnSpawnStack);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }
}
