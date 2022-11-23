using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;

    public static SceneLoader instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SceneLoader>();
                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private Stack<string> Scenes = new Stack<string>();

    public event VoidFunc LoadStarted;
    public event FloatFunc LoadProgress;
    public event VoidFunc LoadFinished;

    void Awake()
    {
        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    IEnumerator AsyncLoad(string sceneId)
    {
        LoadStarted?.Invoke();
        yield return new WaitForEndOfFrame();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            LoadProgress?.Invoke(operation.progress);
            yield return null;
        }
        LoadFinished?.Invoke();
    }

    
    public void LoadScene(string sceneId, bool isNewScene = true)
    {
        if(isNewScene)
        {
            Scenes.Push(SceneManager.GetActiveScene().name);
        }

        StartCoroutine(AsyncLoad(sceneId));
        
    }

    public void LoadPrev()
    {
        LoadScene(Scenes.Pop(), false);
    }
}