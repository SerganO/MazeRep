using UnityEngine;
using System.Linq;

public class ResourcesAudioSupplier : IAudioSupplier
{
    public AudioClip GetAudioForID(string audionId)
    {
        return Resources.Load<AudioClip>("Audio/" + audionId);
    }

    public AudioClip GetAudionForID(string audionId, params string[] subfolders)
    {
        string additionalPath = "";
        subfolders.ToList().ForEach(subfolder => {
            additionalPath += subfolder + "/";
        });
        return GetAudioForID(additionalPath + audionId);
    }
}