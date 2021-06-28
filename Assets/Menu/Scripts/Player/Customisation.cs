using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace cleon
{
    public class Customisation : MonoBehaviour
    {
        [SerializeField] GameObject customisationUI;
        [SerializeField] GameObject mainCamera;
        [SerializeField] GameObject customisationCamera;
        [HideInInspector] public bool isOpen = false;
        GameManager gameManager;

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

        // File path for the customisation save
        string FilePath => Application.streamingAssetsPath + "/CustomData";

        [HideInInspector] public int currentArmourTexture = 0;
        [HideInInspector] public int currentClothesTexture = 0;
        [HideInInspector] public int currentEyesTexture = 0;
        [HideInInspector] public int currentHairTexture = 0;
        [HideInInspector] public int currentMouthTexture = 0;
        [HideInInspector] public int currentSkinTexture = 0;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();

            // If we dont have a path to save the binay file, then create one
            if (!Directory.Exists(Application.streamingAssetsPath))
                Directory.CreateDirectory(Application.streamingAssetsPath);

            // Load the change we save
            LoadCustom();
        }

        private void Update()
        {
            // If there is no menu open then open the custom menu
            if (Input.GetKeyDown(KeyCode.V))
            {
                if(!gameManager.isOpen)
                {
                    customisationUI.SetActive(true);
                    // Use the customisation camera to look at the player and disable the main camera
                    mainCamera.SetActive(false);
                    customisationCamera.SetActive(true);
                    // Load the change we make last time
                    LoadCustom();
                    isOpen = true;
                }
                else if(isOpen)
                {
                    customisationUI.SetActive(false);
                    // Set the main camera back when we close the menu
                    mainCamera.SetActive(true);
                    customisationCamera.SetActive(false);
                    // Save the change we make
                    SaveCustom();
                    isOpen = false;
                }
            }
        }

        public void ChangeArmour()
        {
            // Get a random texture from a armour texture list
            int count = Random.Range(0, armourTexture.Count);
            currentArmourTexture = count;
            // Set the texture to the material that is on the player
            armour.SetTexture("_MainTex", armourTexture[count]);
        }

        public void ChangeClothes()
        {
            // Same as armour
            int count = Random.Range(0, clothesTexture.Count);
            currentClothesTexture = count;
            clothes.SetTexture("_MainTex", clothesTexture[count]);
        }

        public void ChangeEyes()
        {
            // Same as armour
            int count = Random.Range(0, eyesTexture.Count);
            currentEyesTexture = count;
            eyes.SetTexture("_MainTex", eyesTexture[count]);
        }

        public void ChangeHair()
        {
            // Same as armour
            int count = Random.Range(0, hairTexture.Count);
            currentHairTexture = count;
            hair.SetTexture("_MainTex", hairTexture[count]);
        }

        public void ChangeMouth()
        {
            // Same as armour
            int count = Random.Range(0, mouthTexture.Count);
            currentMouthTexture = count;
            mouth.SetTexture("_MainTex", mouthTexture[count]);
        }

        public void ChangeSkin()
        {
            // Same as armour
            int count = Random.Range(0, skinTexture.Count);
            currentSkinTexture = count;
            skin.SetTexture("_MainTex", skinTexture[count]);
        }

        public void SaveCustom()
        {
            // Put the data we wanna save to playerdata and save it
            PlayerData playerData = new PlayerData(this);
            using (FileStream stream = new FileStream(FilePath + ".save", FileMode.OpenOrCreate))
            {
                // Like creating the boat that will carry the data from one point to another
                BinaryFormatter formatter = new BinaryFormatter();
                // Transports the data from the RAM to the specified file, like freezing water
                // into ice.
                formatter.Serialize(stream, playerData);
                stream.Close();
            }
        }

        public void LoadCustom()
        {
            // If there is no save data, we shouldn't attempt to liad it
            if (!File.Exists(FilePath + ".save"))
                return;

            // This opens the 'River' between the RAM and the file
            using (FileStream stream = new FileStream(FilePath + ".save", FileMode.Open))
            {
                // Like creating the boat that will carry the data from one point to another
                BinaryFormatter formatter = new BinaryFormatter();
                // Transports the data from the specified file to the RAM, like unfreezing ice
                // into water.
                PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
                // Get the data and load it and set it
                playerData.LoadPlayerCustom(this);
                armour.SetTexture("_MainTex", armourTexture[currentArmourTexture]);
                clothes.SetTexture("_MainTex", clothesTexture[currentClothesTexture]);
                eyes.SetTexture("_MainTex", eyesTexture[currentEyesTexture]);
                hair.SetTexture("_MainTex", hairTexture[currentHairTexture]);
                mouth.SetTexture("_MainTex", mouthTexture[currentMouthTexture]);
                skin.SetTexture("_MainTex", skinTexture[currentSkinTexture]);
                stream.Close();
            }
        }
    }
}
