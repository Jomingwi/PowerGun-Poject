using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] Button btnGameStart;
    [SerializeField] Button btnGameSet;
    [SerializeField] Button btnGameExit;
    [SerializeField] GameObject StartScene;
    [SerializeField] GameObject objExitCheck;
    [SerializeField] Button btnExitOk;
    [SerializeField] Button btnExitCancel;
       
    [Header("난이도")]
    [SerializeField] Button btnEasy;
    [SerializeField] Button btnNormal;
    [SerializeField] Button btnHard;
    [SerializeField] Button btnDifficultExit;
    [SerializeField] GameObject difficult;

    [Header("단축키")]
    [SerializeField] Button btnKeySetExit;
    [SerializeField] GameObject gameKey;
    


    private void Awake()
    {
        Tool.isStartMainScene = true;

        btnGameStart.onClick.AddListener(gameStart);
        btnGameSet.onClick.AddListener(gameSetting);
        btnGameExit.onClick.AddListener(gameExit);
        btnKeySetExit.onClick.AddListener(gameSetExit);
        btnDifficultExit.onClick.AddListener(difficultSetExit);

        btnEasy.onClick.AddListener(difficultEasy);
        btnNormal.onClick.AddListener(difficultNoraml);
        btnHard.onClick.AddListener(difficultHard);


        difficult.SetActive(false);
        gameKey.SetActive(false);
         objExitCheck.SetActive(false);

  }

    private void difficultEasy()
    {
        SetDifficult(Difficulty.Easy);
    }

    private void difficultNoraml()
    {
        SetDifficult(Difficulty.Normal);
    }

    private void difficultHard()
    {
        SetDifficult(Difficulty.Hard);
    }

    private void SetDifficult(Difficulty difficulty)
    {
        PlayerPrefs.SetInt("DifficultKey", (int)difficulty);
        SceneManager.LoadScene(1);
    }

    private void gameStart()
    {
        StartScene.SetActive(false);
        difficult.SetActive(true);
    }

    private void difficultSetExit()
    {
        difficult.SetActive(false);
        StartScene.SetActive(true);
    }

    private void gameSetting()
    {
        StartScene.SetActive(false);
        gameKey.SetActive(true);
    }
    private void gameSetExit()
    {
        gameKey.SetActive(false);
        StartScene.SetActive(true);
    }



    private void gameExit()
    {
      objExitCheck.SetActive(true);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
    Application.Quit();

#endif

    }
}
