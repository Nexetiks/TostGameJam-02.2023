using System.IO;
using UnityEditor;
using UnityEngine;

public static class CreateAudioData
{
    [MenuItem("Assets/Create/Audio/AudioData")]
    public static void Create()
    {
        var selectedObjects = Selection.objects;
        foreach (Object selectedObject in selectedObjects)
        {
            if (selectedObject is AudioClip audioClip)
            {
                Create(audioClip);
            }
        }
        AssetDatabase.SaveAssets();
    }

    private static void Create(AudioClip audioClip)
    {
        AudioData audioData = ScriptableObject.CreateInstance<AudioData>();

        audioData.AudioClip = audioClip;
        audioData.Volume = 1;

        string path = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject))}/{audioClip.name}.asset";

        AssetDatabase.CreateAsset(audioData, path);
        Selection.activeObject = audioData;
    }
}