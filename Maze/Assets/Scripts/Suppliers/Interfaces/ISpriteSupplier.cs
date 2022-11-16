using UnityEngine;

public interface ISpriteSupplier
{
    public Sprite GetSpriteForID(string spriteId);
    public Sprite GetSpriteForID(string spriteId, params string[] subfolders);
}
