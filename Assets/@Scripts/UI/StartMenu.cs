using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        StartCoroutine(LoadSceneCoro());
    }
    Scene startScene;
    
    IEnumerator LoadSceneCoro()
    {
        startScene = SceneManager.GetActiveScene();
        
        yield return SceneManager.LoadSceneAsync("Global", LoadSceneMode.Additive);
        
        yield return LoadAreaScene("Beach");

        SceneManager.UnloadSceneAsync(startScene);
    }

    private IEnumerator LoadAreaScene(string areaSceneName)
    {
        yield return SceneManager.LoadSceneAsync(areaSceneName, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(areaSceneName));
    }
}
