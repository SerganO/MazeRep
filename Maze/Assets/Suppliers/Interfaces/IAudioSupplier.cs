using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioSupplier
{
    public AudioClip GetAudioForID(string audioId);
    public AudioClip GetAudionForID(string audionId, params string[] subfolders);
}
