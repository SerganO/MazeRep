using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoaderCircle : LoaderView
{
    [SerializeField]
    Image ProgressImage;
    [SerializeField]
    TMP_Text ProgressText;
    public override void LoadStarted()
    {
        base.LoadStarted();
        SetProgress(0);
    }

    public override void LoadProgress(float progress)
    {
        base.LoadProgress(progress);
        SetProgress(progress);
    }

    public override void LoadFinished()
    {
        base.LoadFinished();
        SetProgress(1);
    }

    void SetProgress(float progress)
    {
        ProgressImage.fillAmount = progress;
        ProgressText.text = string.Format("{0:0}%", progress * 100); ;
    }

}
