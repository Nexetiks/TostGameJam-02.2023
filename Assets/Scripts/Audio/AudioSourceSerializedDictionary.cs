using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioSourceSerializedDictionary
{
    [Serializable]
    public struct AudioGroupAudioSourcePair
    {
        public AudioGroup audioGroup;
        public AudioSource audioSource;
    }

    [SerializeField]
    private List<AudioGroupAudioSourcePair> values;

    private Dictionary<AudioGroup, AudioSource> dictionary;
    public Dictionary<AudioGroup, AudioSource> Dictionary
    {
        get
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<AudioGroup, AudioSource>();
                foreach (var x in values)
                {
                    dictionary.Add(x.audioGroup, x.audioSource);
                }
            }
            return dictionary;
        }
    }
}
