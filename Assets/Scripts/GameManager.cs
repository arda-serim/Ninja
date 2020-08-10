using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public Action startGame;

    public Action finishGame;

    GameObject tempUI;

    private void Start()
    {
        startGame += () => Destroy(tempUI);

        tempUI = GameObject.Find("TempUI");
        tempUI.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Best Score\n" + PlayerPrefs.GetInt("BestScore", 0);
    }

    public void StartGame()
    {
        startGame();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
