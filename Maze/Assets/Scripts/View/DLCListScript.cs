using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class DLCListScript : MonoBehaviour
{
    ScrollRect ScrollRect;
    private List<LevelPackData> levelPackDatas = new List<LevelPackData>();

    public RectTransform Content;
    public int lastIndex = 0;

   
    public List<LevelPackItem> levelPackItems = new List<LevelPackItem>();

    public event LevelPackDataFunc SelectedIndexChanged;

    private void Start()
    {
        ScrollRect = GetComponent<ScrollRect>();
        Bind();
        LoadList();
        SetSelection(0);
    }

    void LoadList()
    {
        levelPackItems = new List<LevelPackItem>();
        Content.DestroyAllChilds();
        levelPackDatas = LevelPackManager.sharedInstance.levelPackDatas;
        var levelPackItemTemplate = ResourcesSupplier<LevelPackItem>.PrefabsSupplier.GetObjectForID("LevelPackItem");
        foreach(var data in levelPackDatas)
        {
            var levelPackItemObject = Instantiate(levelPackItemTemplate, Content);
            var levelPackItem = levelPackItemObject.GetComponent<LevelPackItem>();
            levelPackItem.SetupForData(data);
            levelPackItems.Add(levelPackItem);
        }


    }

    public void SetSelection(int index)
    {
        for (int i = 0; i < levelPackItems.Count; i++)
        {
            var item = levelPackItems[i];

           item.SetSize(i == index ? 200 : 150);
        }
        SelectedIndexChanged?.Invoke(levelPackDatas[index]);
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
            SetSelection(lastIndex);
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
            SetSelection(lastIndex);
        }


    }
}
