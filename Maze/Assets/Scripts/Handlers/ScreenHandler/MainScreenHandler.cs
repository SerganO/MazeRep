using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenHandler : MonoBehaviour
{
    public void LoadLevelsScreen()
    {
        SceneLoader.instance.LoadScene("LevelsScreen");
    }
}
