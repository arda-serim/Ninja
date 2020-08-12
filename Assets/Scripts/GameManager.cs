using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoSingleton<GameManager>
{
    public Action startGame;
    public Action finishGame;

    RectTransform tempUI;
    public List<RectTransform> tempUIChilds;

    CinemachineFramingTransposer cinemachineFramingTransposer;

    TextMeshProUGUI gameName;
    TextMeshProUGUI bestScoreText;
    Image settings;

    private void Start()
    {
        startGame += () => Destroy(tempUI.gameObject);

        tempUI = GameObject.Find("TempUI").GetComponent<RectTransform>();
        tempUI.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Best Score\n" + PlayerPrefs.GetInt("BestScore", 0);
        for (int i = 0; i < tempUI.transform.childCount; i++)
        {
            tempUIChilds.Add(tempUI.transform.GetChild(i).gameObject.GetComponent<RectTransform>());
        }
        cinemachineFramingTransposer = GameObject.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();

        gameName = GameObject.Find("GameName").GetComponent<TextMeshProUGUI>();
        bestScoreText = GameObject.Find("BestScore").GetComponent<TextMeshProUGUI>();
        settings = GameObject.Find("Settings").GetComponent<Image>();
        settings.color = new Color(settings.color.r, settings.color.g, settings.color.b, 0);
        gameName.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -0.9f);
        bestScoreText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -0.9f);
        StartCoroutine(GameNameAnimation());
    }

    IEnumerator GameNameAnimation()
    {
        while (gameName.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate) < 0.1f)
        {
            gameName.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, gameName.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate) + 0.005f);
            bestScoreText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, gameName.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate) + 0.005f);
            settings.color = new Color(settings.color.r, settings.color.g, settings.color.b, settings.color.a + 0.005f);
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void StartGame()
    {
        startGame();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator OpenSettings()
    {
        GameObject.Find("TapToStartButton").GetComponent<Button>().enabled = false;
        while (tempUIChilds[2].anchoredPosition.x > -tempUI.rect.width)
        {
            cinemachineFramingTransposer.m_ScreenX -= 0.002f;
            foreach (var item in tempUIChilds)
            {
                item.anchoredPosition -= new Vector2(4,0);
            }
            yield return new WaitForSeconds(0.0001f);
        }
        Image tempImage = tempUIChilds[4].GetComponent<Image>();
        while (tempImage.fillAmount < 1)
        {
            tempImage.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void OpenSettingsCaller()
    {
        StartCoroutine(OpenSettings());
    }

    IEnumerator GoBack()
    {
        Image tempImage = tempUIChilds[4].GetComponent<Image>();
        while (tempImage.fillAmount > 0)
        {
            tempImage.fillAmount -= 0.01f;
            yield return new WaitForSeconds(0.001f);
        }
        while (tempUIChilds[2].anchoredPosition.x < 0)
        {
            cinemachineFramingTransposer.m_ScreenX += 0.002f;
            foreach (var item in tempUIChilds)
            {
                item.anchoredPosition += new Vector2(4f, 0);
            }
            yield return new WaitForSeconds(0.0001f);
        }
        GameObject.Find("TapToStartButton").GetComponent<Button>().enabled = true;
    }

    public void GoBackCaller()
    {
        StartCoroutine(GoBack()); 
    }
}
