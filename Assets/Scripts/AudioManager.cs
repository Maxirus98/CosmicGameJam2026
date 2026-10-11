using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("List of audio sources to manage")]
    private List<AudioClip> audioClipToPlay;

    [SerializeField]
    [Tooltip("Enemy audio clips")]
    private List<AudioClip> audioClipEnemy;

    [SerializeField]
    [Tooltip("Good audio clips")]
    private List<AudioClip> audioClipLover;

    [SerializeField]
    [Tooltip("Push audio clips")]
    private List<AudioClip> pushClip;

    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(int index, float volume)
    {
        if (index < 0 || index >= audioClipToPlay.Count)
        {
            Debug.LogWarning("Audio index out of range.");
            return;
        }

        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClipToPlay[index]);
    }

    public void PlayRandomEnemyClip(float volume)
    {
        if (Instance == null) return;
        if (Instance.audioClipEnemy.Count == 0)
        {
            Debug.LogWarning("No enemy audio clips available.");
            return;
        }

        int randomIndex = Random.Range(0, Instance.audioClipEnemy.Count);
        Instance.audioSource.volume = volume;
        Instance.audioSource.PlayOneShot(Instance.audioClipEnemy[randomIndex]);
    }

    public void PlayRandomLoverClip(float volume)
    {
        if (Instance == null) return;
        if (Instance.audioClipLover.Count == 0)
        {
            Debug.LogWarning("No lover audio clips available.");
            return;
        }

        int randomIndex = Random.Range(0, Instance.audioClipLover.Count);
        Instance.audioSource.volume = volume;
        Instance.audioSource.PlayOneShot(Instance.audioClipLover[randomIndex]);
    }

    public void PlayRandomPushClip(float volume)
    {
        if (Instance == null) return;
        if (Instance.pushClip.Count == 0)
        {
            Debug.LogWarning("No push audio clips available.");
            return;
        }
        int randomIndex = Random.Range(0, Instance.pushClip.Count);
        Instance.audioSource.volume = volume;
        Instance.audioSource.PlayOneShot(Instance.pushClip[randomIndex]);
    }

}
