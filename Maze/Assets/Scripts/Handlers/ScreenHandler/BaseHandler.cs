using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHandler : MonoBehaviour
{
    protected ContextScriptableObject GetCurrentContext()
    {
        return Helper.GetCurrentContext();
    }
}
