using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public GameManager GM;
    public SoundManager MM;

    private Slider _musicSlider;

    // Use this for initialization
    void Start()
    {
        //--------------------------------------------------------------------------
        // Game Settings Related Code


        //--------------------------------------------------------------------------
        // Music Settings Related Code
        _musicSlider = GameObject.Find("Music_Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
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

    //-----------------------------------------------------------
    // Game Options Function Definitions
    //public void OptionSliderUpdate(float val) { ... }
    //void SetCustomSettings(bool val) { ... }
    //void WriteSettingsToInputText(GameSettings settings) { ... }

    //-----------------------------------------------------------
    // Music Settings Function Definitions
    public void MusicSliderUpdate(float val)
    {
        MM.SetVolume(val);
    }

    public void MusicToggle(bool val)
    {
        _musicSlider.interactable = val;
        MM.SetVolume(val ? _musicSlider.value : 0f);
    }
}
