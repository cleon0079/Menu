using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixer mixer;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Dropdown resolusion;
    public Resolution[] resolutions;

    private void Awake()
    {
        LoadPlayerPrefs();
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolusion.ClearOptions();
        List<string> options = new List<string>();

        int currentRseolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)//go through every resolution
        {

            //build a string for displaying the resolution
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                //we have found the current screen resolution, save that number
                currentRseolutionIndex = i;
            }
        }
        //set up our dropdown
        resolusion.AddOptions(options);
        resolusion.value = currentRseolutionIndex;
        resolusion.RefreshShownValue();

        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
            Screen.fullScreen = true;
        }
        else
        {
            if (PlayerPrefs.GetInt("Fullscreen") == 1)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
        }

        if (!PlayerPrefs.HasKey("Quality"))
        {
            PlayerPrefs.SetInt("Quality", 5);
            QualitySettings.SetQualityLevel(5);
        }
        else
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
        }

        PlayerPrefs.Save();
    }

    public void SetFullScreen(bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
    }

    public void ChangeQuality(int _index)
    {
        QualitySettings.SetQualityLevel(_index);
    }

    public void SetMusicVolume(float _value)
    {
        mixer.SetFloat("MusicVol", _value);
    }

    public void SetSFXVolume(float _value)
    {
        mixer.SetFloat("SFXVol", _value);
    }

    public void SetResolution(int _resolutionIndex)
    {
        Resolution res = resolutions[_resolutionIndex];
        Screen.SetResolution(res.width, res.height, false);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());

        if (fullscreenToggle.isOn)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }

        float musicVol;
        if (mixer.GetFloat("MusicVol", out musicVol))
        {
            PlayerPrefs.SetFloat("MusicVol", musicVol);
        }

        float SFXVol;
        if (mixer.GetFloat("SFXVol", out SFXVol))
        {
            PlayerPrefs.SetFloat("SFXVol", SFXVol);
        }

        PlayerPrefs.Save();
    }

    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Quality"))
        {
            int quality = PlayerPrefs.GetInt("Quality");
            qualityDropdown.value = quality;
            if (QualitySettings.GetQualityLevel() != quality)
            {
                ChangeQuality(quality);
            }
        }

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            if (PlayerPrefs.GetInt("Fullscreen") == 0)
            {
                fullscreenToggle.isOn = false;
            }
            else
            {
                fullscreenToggle.isOn = true;
            }
        }
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVol");
            musicSlider.value = musicVol;
            mixer.SetFloat("MusicVol", musicVol);
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            float SFXVol = PlayerPrefs.GetFloat("SFXVol");
            sfxSlider.value = SFXVol;
            mixer.SetFloat("SFXVol", SFXVol);
        }
    } 
}
