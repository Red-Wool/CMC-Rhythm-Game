using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadAssetBundle : MonoBehaviour
{
    public static TextAsset GetSongData(string fileName)
    {
        AssetBundle localAssetBundle =
            AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "songdata"));

        if (localAssetBundle == null)
        {
            Debug.LogError("Failed to load song data: " + fileName);
            return null;
        }

        TextAsset asset = localAssetBundle.LoadAsset<TextAsset>(fileName);
        localAssetBundle.Unload(false);

        return asset;
    }

    public static AudioClip GetMusic(string fileName)
    {
        AssetBundle localAssetBundle =
            AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "music"));

        if (localAssetBundle == null)
        {
            Debug.LogError("Failed to load song data: " + fileName);
            return null;
        }

        AudioClip asset = localAssetBundle.LoadAsset<AudioClip>(fileName);
        localAssetBundle.Unload(false);

        return asset;
    }
}
