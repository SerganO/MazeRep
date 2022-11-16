using UnityEngine;
using System.Linq;

public class ResourcesSpriteSupplier : ISpriteSupplier
{
    public Sprite GetSpriteForID(string spriteId)
    {
        return Resources.Load<Sprite>("Sprites/" + spriteId);
    }

    public Sprite GetSpriteForID(string spriteId, params string[] subfolders)
    {
        string additionalPath = "";
        subfolders.ToList().ForEach(subfolder => {
            additionalPath += subfolder + "/";
        });
        return GetSpriteForID(additionalPath + spriteId);
    }
}