using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishManager : MonoBehaviour
{
    GameManager manager;
    GameData data;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        data = manager.data;
    }

    void OnSetFinishLine()
    {
        manager.intFloats.stackCount = data.LevelStackCounts[data.levelCount];
        manager.bools._canPlaceStack = true;

        //GET FINISH OBJ POS
        GameObject finishObj = Instantiate(Resources.Load<GameObject>("Finish"));

        //GET FINISH OBJ BOUNDSZ IN FLOAT
        float prefabBoundZ = manager.stackGenerator.stackPrefab.transform.localScale.z;
        float stackHeight = StackMovement.CurrentStack.transform.position.y;

        //GET FINISH OBJ POSZ IN FLOAT
        float finishPosZ = manager.stackGenerator.startStack.transform.position.z + (((data.LevelStackCounts[data.levelCount] + 1) * prefabBoundZ) - prefabBoundZ / 5);
        Vector3 finishPos = new Vector3(manager.objects.player.transform.position.x, -30, finishPosZ);

        Vector3 desiredPos = finishPos;
        desiredPos.y = stackHeight + 0.51f;

        //SET FINISH OBJ POS
        finishObj.transform.position = finishPos;
        finishObj.transform.DOMove(desiredPos, 4f).SetEase(Ease.OutCubic);

        manager.objects.finishObj = finishObj;

        //SET FINISH CAM POS
        Vector3 camDesiredPos = desiredPos;
        camDesiredPos.y = 5;
        CameraManager.Instance.CMFinishLine.transform.parent.position = camDesiredPos;
    }



    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSetFinishLine, OnSetFinishLine);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSetFinishLine, OnSetFinishLine);
    }

}
