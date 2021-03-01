using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this);
    }

    void Start()
    {

    }

    // 게임 씬 로드
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
    // 인트로 씬 로드
    public void LoadIntroScene()
    {
        SceneManager.LoadScene(0);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
