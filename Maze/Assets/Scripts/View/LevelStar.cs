using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStar : MonoBehaviour
{
    public GameObject Star;
    public void SetStarReached(bool value)
    {
        Star.SetActive(value);
    }
}
