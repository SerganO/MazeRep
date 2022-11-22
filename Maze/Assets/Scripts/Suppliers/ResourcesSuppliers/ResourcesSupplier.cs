using UnityEngine;
using System.Linq;

public class ResourcesSupplier<T> : ISupplier<T> where T : UnityEngine.Object
{
    public static ResourcesSupplier<Sprite> SpriteSupplier = new ResourcesSupplier<Sprite>("Sprites");
    public static ResourcesSupplier<AudioClip> AudioSupplier = new ResourcesSupplier<AudioClip>("Audios");
    public static ResourcesSupplier<GameObject> PrefabsSupplier = new ResourcesSupplier<GameObject>("Prefabs");
    private string baseFolder;

    public ResourcesSupplier(string baseFolder)
    {
        this.baseFolder = baseFolder;
    }

    public T GetObjectForID(string objectId)
    {
        return Resources.Load<T>(baseFolder + "/" + objectId);
    }

    public T GetObjectForID(string objectId, params string[] subfolders)
    {
        string additionalPath = "";
        subfolders.ToList().ForEach(subfolder => {
            additionalPath += subfolder + "/";
        });
        return GetObjectForID(additionalPath + objectId);
    }
}
