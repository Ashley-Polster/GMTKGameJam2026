using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] string nameOfCurrentScene;

    void Awake()
    {
        instance = this;
    }
    public void LoadSceneByInt(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void LoadSceneByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator loadScreenAfterTime(float time, string sceneName)
    {
        yield return new WaitForSecondsRealtime(time);
        LoadSceneByString(sceneName);
        yield return null;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
