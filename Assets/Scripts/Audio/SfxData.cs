using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SfxData", menuName = "Audio/SfxData", order = 0)]
public class SfxData : ScriptableObject
{
    public AudioData[] AudioDatas;
    public float Volume = 1;
    public float PitchRandomization = 0f;
    public AudioGroup AudioGroup = AudioGroup.Sfx;

    private List<int> unusedIndexes = new List<int>();

    [ContextMenu("Play")]
    public void Play()
    {
        if (unusedIndexes.Count == 0)
        {
            unusedIndexes = Enumerable.Range(0, AudioDatas.Length).ToList();
        }
        int newRandomIndex = unusedIndexes[Random.Range(0, unusedIndexes.Count)];
        unusedIndexes.Remove(newRandomIndex);

        AudioData audioData = AudioDatas[newRandomIndex];
        AudioManager.Instance.Play(audioData.AudioClip, audioData.Volume * Volume, PitchRandomization, AudioGroup);
    }
}
