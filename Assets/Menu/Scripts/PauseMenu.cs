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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause == false)
            {
                PauseGame();
            }
            else
            {
                UnPauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPause = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnPauseGame()
    {
        isPause = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }

    public void BackToMainMenu(int _sceneIndex)
    {
        StartCoroutine(LoadScene(_sceneIndex));
    }

    IEnumerator LoadScene(int _sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            progressBar.fillAmount = progress;
            progressBarText.text = Mathf.RoundToInt(progress * 100) + "%";

            yield return null;
        }
    }
}
