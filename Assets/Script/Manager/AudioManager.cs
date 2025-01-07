using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<AssetReference> soundEffects = new List<AssetReference>();
    public List<AssetReference> backgroundMusic = new List<AssetReference>();

    private Dictionary<string, AudioClip> loadedSFX = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> loadedBGM = new Dictionary<string, AudioClip>();

    private AudioClip currentBGM; // Keeps track of the current BGM


    private async void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Preload AudioClips
        loadedSFX = await LoadAudioClipsAsync(soundEffects);
        loadedBGM = await LoadAudioClipsAsync(backgroundMusic);
        Debug.Log(loadedBGM.Count);
        Debug.Log(loadedSFX.Count);



    }

    private async Task<Dictionary<string, AudioClip>> LoadAudioClipsAsync(List<AssetReference> references)
    {
        Dictionary<string, AudioClip> targetDict = new Dictionary<string, AudioClip>();

        foreach (var reference in references)
        {
            var handle = reference.LoadAssetAsync<AudioClip>();
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var clip = handle.Result;
                if (!targetDict.ContainsKey(clip.name))
                {
                    targetDict.Add(clip.name, clip);
                }
                else
                {
                    Debug.LogWarning($"Duplicate clip name found: {clip.name}. Skipping.");
                }
            }
            else
            {
                Debug.LogWarning($"Failed to load AudioClip from Addressable: {reference.RuntimeKey}");
            }
        }

        return targetDict;
    }

    // Play BGM
    public void PlayBGM(string clipName, float volume = 1.0f, bool loop = false)
    {
        if (loadedBGM.TryGetValue(clipName, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.volume = volume;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{clipName}' not found!");
        }
    }

    // Play Random BGM
    public IEnumerator PlayRandomBGM(float volume = 1.0f, bool loop = false)
    {
       
        yield return new WaitUntil(() => loadedBGM.Count > 0);

        Debug.Log(loadedBGM.Count);
        AudioClip nextBGM;
        do
        {
            nextBGM = GetRandomClip(loadedBGM);
        } while (nextBGM == currentBGM);

        currentBGM = nextBGM;
        PlayClip(currentBGM, volume, loop);

    }   

    // Stop BGM
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // Play an AudioClip
    private void PlayClip(AudioClip clip, float volume, bool loop)
    {
        bgmSource.clip = clip;
        bgmSource.volume = volume;
        bgmSource.loop = loop;
        bgmSource.Play();

        // Listen for the end of the clip if not looping
        if (!loop)
        {
            Debug.Log("AudioClip " + clip.length);
            StartCoroutine(WaitForBGMToEnd(clip.length));
        }
    }

    // Play SFX
    public void PlaySFX(string clipName, float volume = 1.0f)
    {
        if (loadedSFX.TryGetValue(clipName, out var clip))
        {
            sfxSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"SFX '{clipName}' not found!");
        }
    }

    // Set BGM Volume
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    // Set SFX Volume
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    // Utility: Get a random AudioClip from a dictionary
    private AudioClip GetRandomClip(Dictionary<string, AudioClip> clips)
    {
        var keys = new List<string>(clips.Keys);
        var randomKey = keys[Random.Range(0, keys.Count)];
        Debug.Log(randomKey);
        return clips[randomKey];
    }
    private IEnumerator WaitForBGMToEnd(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        StopBGM();
        StartCoroutine(PlayRandomBGM());
    }
}
