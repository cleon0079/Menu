using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    // Image and text that shows the loading progress
    [SerializeField] Image progressBar;
    [SerializeField] Text progressBarText;

    public void StartGame(int _sceneIndex)
    {
        // When we press the start button then load the loading progress
        StartCoroutine(LoadScene(_sceneIndex));
    }

    public void QuitGame()
    {
        // Quit the game in editor or build
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
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
