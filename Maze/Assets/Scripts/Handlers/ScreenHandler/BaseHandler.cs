using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHandler : MonoBehaviour
{
    protected ResourcesSupplier<ContextScriptableObject> ContextSupplier = new ResourcesSupplier<ContextScriptableObject>("Context");

    protected ContextScriptableObject GetCurrentContext()
    {
        return ContextSupplier.GetObjectForID("CurrentContext");
    }
}
