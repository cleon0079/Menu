using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Image progressBar;
    [SerializeField] Text progressBarText;
    [SerializeField] GameObject pauseMenu;
    bool isPause = false;

    [SerializeField] GameObject gamePlayUI;
    [SerializeField] Text playerNameText;
    [SerializeField] InputField inputField;

    private void Start()
    {
        LoadGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // If we press ESC then run pause game function
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(isPause);
        }
    }

    public void PauseGame(bool _isPause)
    {
        // Convert the ispause bool to a int
        // Because if ispause is ture it will return 1 else return 0
        float timeScale = System.Convert.ToInt32(_isPause);
        // Start and pause the game time
        Time.timeScale = timeScale;
        // Open and close the gameplay UI
        gamePlayUI.SetActive(_isPause);
        // Open and close the pause menu
        pauseMenu.SetActive(!_isPause);
        // Show or lock the cursor
        if (!_isPause)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = !_isPause;
        isPause = !_isPause;
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
        if(inputField.text != "")
        {
            playerNameText.text = inputField.text;
            PlayerPrefs.SetString("PlayerName", playerNameText.text);
            PlayerPrefs.Save();
        }
    }

    public void LoadGame()
    {
        // If we have set a players name then load it
        if(PlayerPrefs.HasKey("PlayerName"))
        {
            playerNameText.text = PlayerPrefs.GetString("PlayerName");
        }
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
