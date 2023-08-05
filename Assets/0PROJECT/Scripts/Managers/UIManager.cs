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

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(TypeTexts), .2f, .2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextLevel()
    {
        EventManager.Broadcast(GameEvent.OnNextLevel);
        winPanel.SetActive(false);
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
        yield return new WaitForSeconds(1.5f);
        panel.SetActive(true);
    }

    void TypeTexts()
    {
        scoreText.text = "SCORE: " + manager.data.score;
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
