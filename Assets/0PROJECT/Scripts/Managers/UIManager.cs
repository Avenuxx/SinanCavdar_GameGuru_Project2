using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameManager manager;
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI levelCountText;
    public GameObject[] panelElements;

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
        winPanel.SetActive(false);
        SetPanelElements(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnWin()
    {
        StartCoroutine(OpenWinLosePanel(winPanel));
    }

    private void OnLose()
    {
        StartCoroutine(OpenWinLosePanel(losePanel));
    }

    IEnumerator OpenWinLosePanel(GameObject panel)
    {
        yield return new WaitForSeconds(3f);
        SetPanelElements(false);
        panel.SetActive(true);
    }

    void SetPanelElements(bool active)
    {
        foreach (GameObject element in panelElements)
        {
            element.SetActive(active);
        }
    }

    void TypeTexts()
    {
        scoreText.text = manager.data.Score.ToString("0");
        totalMoneyText.text = manager.data.TotalMoney.ToString("0");
        levelCountText.text = "LEVEL " + (manager.data.levelCount + 1);
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
