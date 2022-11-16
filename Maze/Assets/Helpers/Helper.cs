using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public delegate void VoidFunc();

public static class Helper
{
    public static T DeepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
    public static IEnumerator Wait(float time, VoidFunc action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}

public static class StringHelper
{
    public static string ColorText(this string text, string color)
    {
        return "<color=" + color + ">" + text + "</color>";
    }

    public static string RedText(this string text)
    {
        return text.ColorText("red");
    }

    public static string BlueText(this string text)
    {
        return text.ColorText("blue");
    }

    public static string GreenText(this string text)
    {
        return text.ColorText("green");
    }

    public static string YellowText(this string text)
    {
        return text.ColorText("yellow");
    }
}

public static class TransformHelper
{
    public static void DestroyAllChilds(this Transform transform)
    {
        foreach(Transform child in transform)
        {
            Object.Destroy(child.gameObject);
        }
    }
}
