using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCellInfoItem : MonoBehaviour
{
    public TMP_Text LevelText;
    public List<LevelStar> Stars = new List<LevelStar>();

    public void SetLevelText(string text)
    {
        LevelText.text = text;
    }
    public void SetStar(int reaschedStars)
    {
        for(int i =0; i<reaschedStars;i++)
        {
            Stars[i].SetStarReached(true);
        }

        for(int i= reaschedStars; i<Stars.Count;i++)
        {
            Stars[i].SetStarReached(false);
        }
    }
}
