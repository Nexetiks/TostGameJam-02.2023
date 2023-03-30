using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TierableAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioTierData[] tiers;

    private AudioTierData currentTier;

    private void Start()
    {
        SetTier(0);
        StartCoroutine(PlayAudioCoroutine());
    }

    public void SetTier(int tier)
    {
        currentTier = tiers[Mathf.Clamp(tier, 0, tiers.Length)];
    }

    private IEnumerator PlayAudioCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(6f);
            AudioData audioData = currentTier.Take();
            if (audioData != null)
            {
                audioSource.PlayOneShot(audioData.AudioClip, audioData.Volume);
                yield return new WaitForSeconds(audioData.AudioClip.length + UnityEngine.Random.Range(currentTier.MinDelay, currentTier.MaxDelay));
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    [ContextMenu("SeTier0")]
    private void SetTier0()
    {
        SetTier(0);
    }

    [ContextMenu("SetTier1")]
    private void SetTier1()
    {
        SetTier(1);
    }

    [ContextMenu("SetTier2")]
    private void SetTier2()
    {
        SetTier(2);
    }

    public void SetValue(int workersAmount)
    {
        AudioTierData tier = tiers.LastOrDefault(x => x.MinValueToEnableTier <= workersAmount);

        if (tier != null)
        {
            currentTier = tier;
        }
    }
}
