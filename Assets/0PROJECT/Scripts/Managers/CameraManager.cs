using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : InstanceManager<CameraManager>
{
    public CMCam cMCamEnum;
    public GameObject CMPlayer, CMFinishLine;
    public List<GameObject> CamList = new List<GameObject>();

    private void Start()
    {
        InitializeCamList();
        InvokeRepeating("CamControl", 0.1f, 0.1f);
    }

    private void InitializeCamList()
    {
        CamList.Add(CMPlayer);
        CamList.Add(CMFinishLine);
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

    private void OnFinishLine()
    {
        cMCamEnum = CMCam.CMFinishLine;
    }

    // ###############################     EVENTS      ###################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnFinishLine, OnFinishLine);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFinishLine, OnFinishLine);
    }
}
