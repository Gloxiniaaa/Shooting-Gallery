using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeSO", menuName = "CameraShakeSO")]
public class CameraShakeSO : ScriptableObject
{
    [Range(0.1f, 1f)]
    public float Duration;

    [Range(0, 0.3f)]
    public float Strength;

    [Range(5, 10)]
    public int Vibrato;

    [Range(0, 90)]
    public float Randomness;
}