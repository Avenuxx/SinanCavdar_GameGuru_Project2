using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : InstanceManager<CameraManager>
{
    GameManager manager;
    public CMCam cMCamEnum;
    public GameObject CMPlayer, CMFinishLine, CMLose;
    public List<GameObject> CamList = new List<GameObject>();

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        InitializeCamList();
        InvokeRepeating("CamControl", 0.1f, 0.1f);
    }

    private void InitializeCamList()
    {
        CamList.Add(CMPlayer);
        CamList.Add(CMFinishLine);
        CamList.Add(CMLose);
    }

    private void CamControl()
    {
        GameObject activeCam = GetActiveCamObject();
        UpdateCamList(activeCam);
    }

    private GameObject GetActiveCamObject()
    {
        switch (cMCamEnum)
        {
            case CMCam.CMPlayer:
                return CMPlayer;

            case CMCam.CMFinishLine:
                return CMFinishLine;

            case CMCam.CMLose:
                return CMLose;

            default:
                return CMPlayer;
        }
    }

    private void UpdateCamList(GameObject activeCam)
    {
        foreach (GameObject cam in CamList)
        {
            bool isActive = cam == activeCam;
            cam.SetActive(isActive);
        }
    }

    private void OnWin()
    {
        CMFinishLine.transform.parent.GetComponent<Animator>().enabled = true;
        CMFinishLine.transform.parent.GetComponent<Animator>().Play("FinishCamAnim", 0, 0);
        cMCamEnum = CMCam.CMFinishLine;
    }

    private void OnNextLevel()
    {
        CMFinishLine.transform.parent.GetComponent<Animator>().enabled = false;
        cMCamEnum = CMCam.CMPlayer;
    }

    private void OnLose()
    {
        CMLose.transform.position = manager.objects.player.transform.position + new Vector3(5, 10, -10);
        cMCamEnum = CMCam.CMLose;
    }

    // ###############################     EVENTS      ###################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnWin, OnWin);
        EventManager.AddHandler(GameEvent.OnLose, OnLose);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnWin, OnWin);
        EventManager.RemoveHandler(GameEvent.OnLose, OnLose);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }
}
