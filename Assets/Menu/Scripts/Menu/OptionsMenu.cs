using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace cleon
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;
        [SerializeField] AudioMixer mixer;
        [SerializeField] Dropdown qualityDropdown;
        [SerializeField] Toggle fullscreenToggle;
        [SerializeField] Dropdown resolusion;
        [SerializeField] Resolution[] resolutions;

        private void Awake()
        {
            LoadPlayerPrefs();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Get the monitors resolutions and save it in our array
            resolutions = Screen.resolutions;
            // Clear all the options in the resolusion dropdown and make a list of string for the resolusion options
            resolusion.ClearOptions();
            List<string> options = new List<string>();

            int currentRseolutionIndex = 0;
            // Go through every resolution
            for (int i = 0; i < resolutions.Length; i++)
            {
                // Build a string for displaying the resolution
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    // We have found the current screen resolution, save that number
                    currentRseolutionIndex = i;
                }
            }
            // Set up our dropdown
            resolusion.AddOptions(options);
            resolusion.value = currentRseolutionIndex;
            resolusion.RefreshShownValue();

            // The default value of the fullscreen is ture
            if (!PlayerPrefs.HasKey("Fullscreen"))
            {
                PlayerPrefs.SetInt("Fullscreen", 1);
                Screen.fullScreen = true;
            }
            else
            {
                // Set the fullscreen
                if (PlayerPrefs.GetInt("Fullscreen") == 1)
                {
                    Screen.fullScreen = true;
                }
                else
                {
                    Screen.fullScreen = false;
                }
            }

            // Set the default value of quality as Very High
            if (!PlayerPrefs.HasKey("Quality"))
            {
                PlayerPrefs.SetInt("Quality", 5);
                QualitySettings.SetQualityLevel(5);
            }
            else
            {
                // If we have set the quality then load it
                QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
            }

            // Save the default setting
            PlayerPrefs.Save();
        }

        public void SetFullScreen(bool _fullscreen)
        {
            // Change it to fullscreen or windows
            Screen.fullScreen = _fullscreen;
        }

        public void ChangeQuality(int _index)
        {
            // Change the quality level
            QualitySettings.SetQualityLevel(_index);
        }

        public void SetMusicVolume(float _value)
        {
            // Change the music volume from -80 to 0
            mixer.SetFloat("MusicVol", _value);
        }

        public void SetSFXVolume(float _value)
        {
            // Change the SFX volume from -80 to 0
            mixer.SetFloat("SFXVol", _value);
        }

        public void SetResolution(int _resolutionIndex)
        {
            // Change the resolution
            Resolution res = resolutions[_resolutionIndex];
            Screen.SetResolution(res.width, res.height, fullscreenToggle.isOn);
        }

        // When we leave the options menu we run this function
        public void SavePlayerPrefs()
        {
            // Save the quality level to a int
            PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());

            // If is fullscreen then save the value to 1 else 0
            if (fullscreenToggle.isOn)
            {
                PlayerPrefs.SetInt("Fullscreen", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Fullscreen", 0);
            }

            // Get the value of the music volume and save it as a float
            float musicVol;
            if (mixer.GetFloat("MusicVol", out musicVol))
            {
                PlayerPrefs.SetFloat("MusicVol", musicVol);
            }

            // Get the value of the SFX volume and save it as a float
            float SFXVol;
            if (mixer.GetFloat("SFXVol", out SFXVol))
            {
                PlayerPrefs.SetFloat("SFXVol", SFXVol);
            }

            PlayerPrefs.Save();
        }

        public void LoadPlayerPrefs()
        {
            // If we have set the quality level before then set the level to the one last time and show it on dropdown
            if (PlayerPrefs.HasKey("Quality"))
            {
                int quality = PlayerPrefs.GetInt("Quality");
                qualityDropdown.value = quality;
                if (QualitySettings.GetQualityLevel() != quality)
                {
                    ChangeQuality(quality);
                }
            }

            // If we have set the fullscreen before then set the one we changed
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

            // If we changed the music volume before then set it 
            if (PlayerPrefs.HasKey("MusicVol"))
            {
                float musicVol = PlayerPrefs.GetFloat("MusicVol");
                musicSlider.value = musicVol;
                mixer.SetFloat("MusicVol", musicVol);
            }

            // If we changed the SFX volume before then set it
            if (PlayerPrefs.HasKey("SFXVol"))
            {
                float SFXVol = PlayerPrefs.GetFloat("SFXVol");
                sfxSlider.value = SFXVol;
                mixer.SetFloat("SFXVol", SFXVol);
            }
        }
    }
}
