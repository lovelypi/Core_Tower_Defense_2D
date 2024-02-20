using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    
    public void LoadScenePro(string sceneName)
    {
        StartCoroutine(IELoadScene(sceneName));
    }

    public void ReloadScene()
    {
        var curSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(curSceneName);
    }

    public void ReloadScenePro()
    {
        var curSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(IELoadScene(curSceneName));
    }

    public IEnumerator IELoadScene(string sceneName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
