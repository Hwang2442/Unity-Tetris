using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelSetting;    // 설정화면
    [SerializeField]
    private GameObject panelOver;       // 종료화면

    private IEnumerator Start()
    {
        yield return null;

        SoundManager.Instance.Play("BGM-2");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelSetting != null)
            {
                ShowPanelSetting(!panelSetting.activeSelf);
            }
        }
    }

    public void LoadIntroScene()
    {
        SoundManager.Instance.Play("BGM-2", true);

        SceneManager.LoadScene("IntroScene");

        //Time.timeScale = 1;
    }

    public void LoadGameScene()
    {
        //SoundManager.Instance.Play("BGM-2");

        SceneManager.LoadScene("GameScene");

        //Time.timeScale = 1;
    }

    public void GameQuit()
    {
        SoundManager.Instance.PlayOneShot("FX_Common");

        Application.Quit();
    }

    public void ShowPanelOver(bool active)
    {
        SoundManager.Instance.PlayOneShot("FX_Common");

        if (panelOver != null)
        {
            panelOver.SetActive(active);

            //Time.timeScale = !active ? 1 : 0;
        }
    }

    public void ShowPanelSetting(bool active)
    {
        SoundManager.Instance.PlayOneShot("FX_Common");

        if (panelSetting != null)
        {
            panelSetting.SetActive(active);

            Time.timeScale = !active ? 1 : 0;
        }
    }

    public void VolumeSync(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }
}
