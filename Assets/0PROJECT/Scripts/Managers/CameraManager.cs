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

    // ###############################     EVENTS      ###################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnWin, OnWin);
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnWin, OnWin);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }
}
