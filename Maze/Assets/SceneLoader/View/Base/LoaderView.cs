using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderView : MonoBehaviour
{
    private void Bind()
    {
        SceneLoader.instance.LoadStarted += LoadStarted;
        SceneLoader.instance.LoadProgress += LoadProgress;
        SceneLoader.instance.LoadFinished += LoadFinished;
    }

    private void Unbind()
    {
        SceneLoader.instance.LoadStarted -= LoadStarted;
        SceneLoader.instance.LoadProgress -= LoadProgress;
        SceneLoader.instance.LoadFinished -= LoadFinished;
    }

    // Start is called before the first frame update
    void Start()
    {
        Bind();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Unbind();
    }

    public virtual void LoadStarted()
    {
        Debug.Log("Scene load started");
    }

    public virtual void LoadProgress(float progress)
    {
        string progressPercentString = string.Format("{0:0}%", progress / 0.9 * 100);
        Debug.Log("Scene load progress: " + progressPercentString);
    }

    public virtual void LoadFinished()
    {
        Debug.Log("Scene load finished");
    }
}
