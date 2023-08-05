using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameManager manager;

    [Serializable]
    public struct Objects
    {
        public GameObject winPanel;
        public GameObject losePanel;
        public GameObject[] panelElements;
    }

    [Serializable]
    public struct Texts
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI totalMoneyText;
        public TextMeshProUGUI levelCountText;
        public TextMeshProUGUI winPerfectStreakText;
        public TextMeshProUGUI losePerfectStreakText;
    }

    public Objects objects;
    public Texts texts;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        InvokeRepeating(nameof(TypeTexts), 0, .2f);
    }

    public void NextLevel()
    {
        EventManager.Broadcast(GameEvent.OnNextLevel);
        objects.winPanel.SetActive(false);
        EventManager.Broadcast(GameEvent.OnPlaySound, "Pop");
        SetPanelElements(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnWin()
    {
        StartCoroutine(OpenWinLosePanel(objects.winPanel));
        EventManager.Broadcast(GameEvent.OnPlaySound, "Success");
    }

    private void OnLose()
    {
        StartCoroutine(OpenWinLosePanel(objects.losePanel));
    }

    IEnumerator OpenWinLosePanel(GameObject panel)
    {
        yield return new WaitForSeconds(3f);
        SetPanelElements(false);
        panel.SetActive(true);
    }

    void SetPanelElements(bool active)
    {
        foreach (GameObject element in objects.panelElements)
        {
            element.SetActive(active);
        }
    }

    void TypeTexts()
    {
        //CONTROL OF TEXTS IN REPEAT
        texts.scoreText.text = manager.data.Score.ToString("0");
        texts.totalMoneyText.text = manager.data.TotalMoney.ToString("0");
        texts.levelCountText.text = "LEVEL " + (manager.data.levelCount + 1);

        string perfectStreakText = manager.intFloats.perfectStackStreak + " perfect streak / " + manager.data.LevelStackCounts[manager.data.levelCount] + " stack";
        texts.winPerfectStreakText.text = perfectStreakText;
        texts.losePerfectStreakText.text = perfectStreakText;
    }


    ///////////////// EVENTS /////////////////
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnWin, OnWin);
        EventManager.AddHandler(GameEvent.OnLose, OnLose);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnWin, OnWin);
        EventManager.RemoveHandler(GameEvent.OnLose, OnLose);
    }
}
