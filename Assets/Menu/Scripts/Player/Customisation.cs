using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cleon
{
    public class Customisation : MonoBehaviour
    {
        [SerializeField] GameObject playerStatsGO;
        [SerializeField] GameObject customisationUI;
        [SerializeField] GameObject gamePlayUI;
        [SerializeField] GameObject pauseMenuUI;
        [SerializeField] GameObject mainCamera;
        [SerializeField] GameObject customisationCamera;
        PauseMenu pauseMenu;
        Player player;
        public bool isChanging = false;

        [SerializeField] Material armour;
        [SerializeField] Material clothes;
        [SerializeField] Material eyes;
        [SerializeField] Material hair;
        [SerializeField] Material mouth;
        [SerializeField] Material skin;
        [SerializeField] List<Texture> armourTexture;
        [SerializeField] List<Texture> clothesTexture;
        [SerializeField] List<Texture> eyesTexture;
        [SerializeField] List<Texture> hairTexture;
        [SerializeField] List<Texture> mouthTexture;
        [SerializeField] List<Texture> skinTexture;
        int currentArmourTexture = 0;
        int currentClothesTexture = 0;
        int currentEyesTexture = 0;
        int currentHairTexture = 0;
        int currentMouthTexture = 0;
        int currentSkinTexture = 0;

        private void Start()
        {
            pauseMenu = pauseMenuUI.GetComponent<PauseMenu>();
            player = playerStatsGO.GetComponent<Player>();
            LoadPlayerPrefs();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V) && pauseMenu.isPause == false && player.isChanging == false)
            {
                Change(isChanging);
            }
        }

        public void Change(bool _isChanging)
        {
            // Convert the ispause bool to a int
            // Because if ispause is ture it will return 1 else return 0
            float timeScale = System.Convert.ToInt32(_isChanging);
            // Start and pause the game time
            Time.timeScale = timeScale;
            // Open and close the gameplay UI
            gamePlayUI.SetActive(_isChanging);
            // Open and close the pause menu
            customisationUI.SetActive(!_isChanging);
            mainCamera.SetActive(_isChanging);
            customisationCamera.SetActive(!_isChanging);
            // Show or lock the cursor
            if (!_isChanging)
            {
                Cursor.lockState = CursorLockMode.None;
                LoadPlayerPrefs();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                SavePlayerPrefs();
            }
            Cursor.visible = !_isChanging;
            isChanging = !_isChanging;
        }

        public void ChangeArmour()
        {
            int count = Random.Range(0, armourTexture.Count);
            currentArmourTexture = count;
            armour.SetTexture("_MainTex", armourTexture[count]);
        }

        public void ChangeClothes()
        {
            int count = Random.Range(0, clothesTexture.Count);
            currentClothesTexture = count;
            clothes.SetTexture("_MainTex", clothesTexture[count]);
        }

        public void ChangeEyes()
        {
            int count = Random.Range(0, eyesTexture.Count);
            currentEyesTexture = count;
            eyes.SetTexture("_MainTex", eyesTexture[count]);
        }

        public void ChangeHair()
        {
            int count = Random.Range(0, hairTexture.Count);
            currentHairTexture = count;
            hair.SetTexture("_MainTex", hairTexture[count]);
        }

        public void ChangeMouth()
        {
            int count = Random.Range(0, mouthTexture.Count);
            currentMouthTexture = count;
            mouth.SetTexture("_MainTex", mouthTexture[count]);
        }

        public void ChangeSkin()
        {
            int count = Random.Range(0, skinTexture.Count);
            currentSkinTexture = count;
            skin.SetTexture("_MainTex", skinTexture[count]);
        }

        public void SavePlayerPrefs()
        {
            PlayerPrefs.SetInt("Armour", currentArmourTexture);
            PlayerPrefs.SetInt("Clothes", currentClothesTexture);
            PlayerPrefs.SetInt("Eyes", currentEyesTexture);
            PlayerPrefs.SetInt("Hair", currentHairTexture);
            PlayerPrefs.SetInt("Mouth", currentMouthTexture);
            PlayerPrefs.SetInt("Skin", currentSkinTexture);
            PlayerPrefs.Save();
        }

        public void LoadPlayerPrefs()
        {
            if (!PlayerPrefs.HasKey("Armour"))
            {
                PlayerPrefs.SetInt("Armour", currentArmourTexture);
            }
            else
            {
                currentArmourTexture = PlayerPrefs.GetInt("Armour");
            }
            armour.SetTexture("_MainTex", armourTexture[currentArmourTexture]);

            if (!PlayerPrefs.HasKey("Clothes"))
            {
                PlayerPrefs.SetInt("Clothes", currentClothesTexture);
            }
            else
            {
                currentClothesTexture = PlayerPrefs.GetInt("Clothes");
            }
            clothes.SetTexture("_MainTex", clothesTexture[currentClothesTexture]);

            if (!PlayerPrefs.HasKey("Eyes"))
            {
                PlayerPrefs.SetInt("Eyes", currentEyesTexture);
            }
            else
            {
                currentEyesTexture = PlayerPrefs.GetInt("Eyes");
            }
            eyes.SetTexture("_MainTex", eyesTexture[currentEyesTexture]);

            if (!PlayerPrefs.HasKey("Hair"))
            {
                PlayerPrefs.SetInt("Hair", currentHairTexture);
            }
            else
            {
                currentHairTexture = PlayerPrefs.GetInt("Hair");
            }
            hair.SetTexture("_MainTex", hairTexture[currentHairTexture]);

            if (!PlayerPrefs.HasKey("Mouth"))
            {
                PlayerPrefs.SetInt("Mouth", currentMouthTexture);
            }
            else
            {
                currentMouthTexture = PlayerPrefs.GetInt("Mouth");
            }
            mouth.SetTexture("_MainTex", mouthTexture[currentMouthTexture]);

            if (!PlayerPrefs.HasKey("Skin"))
            {
                PlayerPrefs.SetInt("Skin", currentSkinTexture);
            }
            else
            {
                currentSkinTexture = PlayerPrefs.GetInt("Skin");
            }
            skin.SetTexture("_MainTex", skinTexture[currentSkinTexture]);

            PlayerPrefs.Save();
        }
    }
}
