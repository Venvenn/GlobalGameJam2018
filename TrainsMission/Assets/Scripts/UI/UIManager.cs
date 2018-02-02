using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public GameManager GM;
    public SoundManager MM;

    public Slider _musicSlider;

    // Use this for initialization
    void Start()
    {
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


    public void MusicSliderUpdate(float val)
    {
        MM.SetVolume(val);
    }

    public void MusicToggle(bool val)
    {
        _musicSlider.interactable = val;
        MM.SetVolume(val ? _musicSlider.value : 0f);
    }

    public void Load(int Index)
    {
        SceneManager.LoadScene(Index);
    }

    public void Exit()
    {
        Debug.Log("quit");
        Application.Quit();
    }

}
