using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Listen on channel:")]
    [SerializeField] AudioEventChannelSO _sfxEvent;
    [SerializeField] FloatEventChannelSO _sfxVolumnEvent;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _sfxEvent.OnEventRaised += PlayAudio;
        _sfxVolumnEvent.OnEventRaised += ChangeVolumn;
    }

    private void PlayAudio(AudioGroupSO audioGroupSO)
    {
        _audioSource.PlayOneShot(audioGroupSO.GetRandomClip(), audioGroupSO.Volume);
    }

    private void ChangeVolumn(float value)
    {
        _audioSource.volume = value;
    }

    private void OnDisable()
    {
        _sfxEvent.OnEventRaised += PlayAudio;
        _sfxVolumnEvent.OnEventRaised += ChangeVolumn;
    }
}