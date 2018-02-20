using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public GameManager GM;
    public SoundManager MM;
    LevelData levelData;

    public Slider _musicSlider;
    public Toggle _musicMute;

    // Use this for initialization
    void Start()
    {
        levelData = GameObject.Find("LevelManager").GetComponent<LevelData>();
        ToggleSound();
    }


    void Update()
    {
        ScanForKeyStroke();
    }

    void ScanForKeyStroke()
    {
        if (Input.GetKeyDown("escape"))
        {
            GM.TogglePauseMenu();
        }
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void NextLevel()
    {
        if (levelData.CurrentLevel.number >= 4)
        {
            levelData.CurrentLevel = levelData.LevelList[0];
            SceneManager.LoadScene(1);
        }
        else
        {
            levelData.CurrentLevel = levelData.LevelList[levelData.CurrentLevel.number];
            SceneManager.LoadScene(2);
        }
    }

    public void MusicSliderUpdate(float val)
    {
        PlayerPrefs.SetFloat("Volume", val);
        MM.SetVolume(val);
    }

    public void MusicToggle(bool val)
    {
        if (val == true)
        {
            PlayerPrefs.SetInt("Muted", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Muted", 0);
        }

        _musicSlider.interactable = val;
        MM.SetVolume(val ? _musicSlider.value : 0f);
    }
    public void ToggleSound()
    {
        if (!PlayerPrefs.HasKey("Muted"))
        {
            PlayerPrefs.SetInt("Muted", 1);
        }

        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 0.3f);
        }

        _musicSlider.value = PlayerPrefs.GetFloat("Volume");

        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            MM.SetVolume(0f);
            _musicMute.isOn = false;
            _musicSlider.interactable = false;
        }
        else
        {
            _musicMute.isOn = true;
            MM.SetVolume(PlayerPrefs.GetFloat("Volume"));
        }




    }

    public void Load(int Index)
    {
        Destroy(GameObject.Find("LevelManager"));
        SceneManager.LoadScene(Index);
    }

    public void Exit()
    {
        Debug.Log("quit");
        Application.Quit();
    }

}
