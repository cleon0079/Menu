using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace cleon
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] Image progressBar;
        [SerializeField] Text progressBarText;

        [SerializeField] GameObject pauseMenu;
        [SerializeField] GameObject playerGO;
        [HideInInspector] public bool isOpen = false;
        GameManager gameManager;

        [SerializeField] Text playerNameText;
        [SerializeField] InputField inputField;

        string FilePath => Application.streamingAssetsPath + "/gameData";

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            if (!Directory.Exists(Application.streamingAssetsPath))
                Directory.CreateDirectory(Application.streamingAssetsPath);
            LoadGame();
        }

        private void Update()
        {
            // If we press ESC then run pause game function
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gameManager.isOpen)
                {
                    pauseMenu.SetActive(true);
                    isOpen = true;
                }
                else if (isOpen)
                {
                    pauseMenu.SetActive(false);
                    isOpen = false;
                }
            }
        }

        public void QuitGame()
        {
            // Quit the game in editor or build
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }

        public void SaveGame()
        {
            // If we have anything in the inputfield then set it to our players name and save it 
            if (inputField.text != "")
            {
                playerNameText.text = inputField.text;
                PlayerPrefs.SetString("PlayerName", playerNameText.text);
                PlayerPrefs.Save();
            }
            SaveBinary(gameManager.player);
        }

        void SaveBinary(Player _player)
        {
            // Get the data from playerdata
            PlayerData playerData = new PlayerData(_player);
            // This opens the 'River' between the RAM and the file
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

        void LoadBinary(Player _player)
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
                // Load the data and then reset the player POS and ROT
                playerData.LoadPlayerData(_player);
                playerGO.transform.position = _player.playerPos;
                playerGO.transform.rotation = _player.playerRot;
                stream.Close();
            }
        }

        public void LoadGame()
        {
            // If we have set a players name then load it
            if (PlayerPrefs.HasKey("PlayerName"))
            {
                playerNameText.text = PlayerPrefs.GetString("PlayerName");
            }
            LoadBinary(gameManager.player);
        }

        public void BackToMainMenu(int _sceneIndex)
        {
            // When we press the main menu button then load the loading progress
            StartCoroutine(LoadScene(_sceneIndex));
        }

        IEnumerator LoadScene(int _sceneIndex)
        {
            // Get the operation and load the scene we want
            AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneIndex);

            // While operation is not done
            while (!operation.isDone)
            {
                // The loading phase is only between 0 to 0.9 so if i want 0 to 1 then i have to divide by 0.9
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                // Change the progress image fill amount with progress
                progressBar.fillAmount = progress;
                // Show it in a int with text
                progressBarText.text = Mathf.RoundToInt(progress * 100) + "%";

                yield return null;
            }
        }
    }
}
