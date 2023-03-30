using Gameplay;
using General;
using System.Collections;
using UnityEngine;

public class AudioManager : SingletonUnity<AudioManager>
{
    [SerializeField]
    private AudioSourceSerializedDictionary audioSources;
    [SerializeField]
    private TierableAudio whispersTierableAudio;
    [field: SerializeField]
    [SerializeField]
    public SfxData DayMusic { get; private set; }
    [field: SerializeField]
    [SerializeField]
    public SfxData NightMusic { get; private set; }
    [field: SerializeField]
    [SerializeField]
    public Sfxs Sfxs { get; private set; }

    private void Start()
    {
        DayMusic.Play();
        DayPhases.OnDay += DayPhases_OnDay;
        DayPhases.OnNight += DayPhases_OnNight;
        GameManager.Instance.OnWorkerSpawned += GameManager_OnWorkerSpawned;
    }

    private void DayPhases_OnDay()
    {
        StartCoroutine(Coroutine());

        IEnumerator Coroutine()
        {
            Sfxs.Bells.Play();
            yield return new WaitForSeconds(4f);
            DayMusic.Play();
        }
    }

    private void DayPhases_OnNight()
    {
        StartCoroutine(Coroutine());

        IEnumerator Coroutine()
        {
            Sfxs.Hum.Play();
            yield return new WaitForSeconds(6f);
            NightMusic.Play();
        }
    }

    private void GameManager_OnWorkerSpawned(WorkerSpawnData data)
    {
        whispersTierableAudio.SetValue(data.WorekrsAmount);
    }

    public void Play(AudioClip audioClip, float volume = 1, float pitchRandomization = 0, AudioGroup audioGroup = AudioGroup.Sfx)
    {
        audioSources.Dictionary[audioGroup].pitch = 1 + Random.Range(-pitchRandomization, pitchRandomization);
        audioSources.Dictionary[audioGroup].PlayOneShot(audioClip, volume);
    }

    public void SetWhispersTier(int tier)
    {
        whispersTierableAudio.SetTier(tier);
    }
}
