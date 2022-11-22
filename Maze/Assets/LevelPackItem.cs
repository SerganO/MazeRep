using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPackItem : MonoBehaviour
{
    public Image Cover;
    public TMP_Text NameLabel;
    public Image Lock;

    public void SetSize(float value) {
        Cover.GetComponent<RectTransform>().sizeDelta = new Vector2(value, value);
    }

    public void SetLockVisible(bool value)
    {
        Lock.gameObject.SetActive(value);
    }

    public void SetupForData(LevelPackData data)
    {
        SetLockVisible(!data.available);
        var image = ResourcesSupplier<Sprite>.SpriteSupplier.GetObjectForID(data.packId, "LevelPacks");
        if(image != null)
        {
            Cover.sprite = image;
        } else
        {
            Cover.sprite = ResourcesSupplier<Sprite>.SpriteSupplier.GetObjectForID("template", "LevelPacks");
        }
        NameLabel.text = data.packName;
    }

}
