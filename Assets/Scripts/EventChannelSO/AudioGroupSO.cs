using UnityEngine;

[CreateAssetMenu(fileName = "AudioGroupSO", menuName = "AudioGroupSO")]
public class AudioGroupSO : ScriptableObject
{
    [SerializeField] AudioClip[] _audioClips;
    public float Volume;
    
    public AudioClip GetRandomClip()
    {
        return _audioClips[Random.Range(0, _audioClips.Length)];
    }
}   