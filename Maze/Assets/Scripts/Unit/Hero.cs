using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public List<SpriteRenderer> ShadowsTiles;
    public SpriteRenderer Shadow02;
    public SpriteRenderer Shadow11;
    public SpriteRenderer Shadow20;
    public SpriteRenderer Shadow1_1;
    public SpriteRenderer Shadow0_2;
    public SpriteRenderer Shadow_1_1;
    public SpriteRenderer Shadow_20;
    public SpriteRenderer Shadow_11;
    public void CheckShadow()
    {
        Vector3Int pos = levelHandler.currentPosition;
        Shadow02.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int(pos.x, pos.y+2), 0, 2));
        Shadow11.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int(pos.x+1, pos.y+1), 1, 1));
        Shadow20.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int(pos.x+2, pos.y), 2, 0));
        Shadow1_1.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int(pos.x+1, pos.y-1), 1, 1));
        Shadow0_2.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int(pos.x, pos.y-2), 0, 2));
        Shadow_1_1.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int(pos.x-1, pos.y-1), 1, 1));
        Shadow_20.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int(pos.x-2, pos.y), 2, 0));
        Shadow_11.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int(pos.x-1, pos.y+1), 1, 1));

        //foreach(var shadow in ShadowsTiles)
        //{
        //    Vector3Int pos = levelHandler.currentPosition;
        //    int dX = (int)shadow.gameObject.transform.position.x;
        //    int dY = (int)shadow.gameObject.transform.position.y;
        //    var newX = pos.x + dX;
        //    var newY = pos.y + dY;

        //    shadow.gameObject.SetActive(!levelHandler.CanSeeSecondRadiusObject(pos, new Vector3Int((int)newX,(int)newY), 
        //        (int)Mathf.Abs(dX), (int)Mathf.Abs(dY)));
        //}
    }
}
