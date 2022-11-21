using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class DLCListScript : MonoBehaviour
{
    ScrollRect ScrollRect;
    public RectTransform Content;
    public int lastIndex = 0;

    private void Start()
    {
        ScrollRect = GetComponent<ScrollRect>();
        Bind();
        var child = Content.GetChild(lastIndex).GetChild(0);

        child.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
    }
    public void Bind()
    {
        ScrollRect.onValueChanged.AddListener((x) => { ChangeSelected(x); });
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(lastIndex);
            var elementOffset = 1.0f / (Content.childCount - 1);
            ScrollRect.horizontalNormalizedPosition = elementOffset * lastIndex + 0.0001f;
            for (int i = 0; i < Content.childCount; i++)
            {
                var child = Content.GetChild(i).GetChild(0);

                child.GetComponent<RectTransform>().sizeDelta = i == lastIndex ? new Vector2(200, 200) : new Vector2(150, 150);
            }
        }
    }


    public void ChangeSelected(Vector2 value)
    {
        Debug.Log(value);
        var elementOffset = 1.0f / (Content.childCount - 1);

        int index = (int)(value.x / elementOffset);

        if (index != lastIndex)
        {
            lastIndex = index;
            for (int i = 0; i < Content.childCount; i++)
            {
                var child = Content.GetChild(i).GetChild(0);

                child.GetComponent<RectTransform>().sizeDelta = i == index ? new Vector2(200, 200) : new Vector2(150, 150);
            }
        }


    }
}
