using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Image progressBar;
    public Text progressBarText;

    public void StartGame(int _sceneIndex)
    {
        StartCoroutine(LoadScene(_sceneIndex));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
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
