using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioTierData", menuName = "Audio/AudioTierData", order = 0)]
public class AudioTierData : ScriptableObject
{
    public int MinValueToEnableTier = 0;
    public float MinDelay = 5;
    public float MaxDelay = 10;
    public AudioData[] AudioDatas;

    private List<int> unusedIndexes = new List<int>();

    public AudioData Take()
    {
        if (AudioDatas.Length > 0)
        {
            if (unusedIndexes.Count == 0)
            {
                unusedIndexes = Enumerable.Range(0, AudioDatas.Length).ToList();
            }
            int newRandomIndex = unusedIndexes[Random.Range(0, unusedIndexes.Count)];
            unusedIndexes.Remove(newRandomIndex);

            //TODO: losowanie bez powtorzen
            return AudioDatas[newRandomIndex];
        }
        else
        {
            return null;
        }
    }
}

