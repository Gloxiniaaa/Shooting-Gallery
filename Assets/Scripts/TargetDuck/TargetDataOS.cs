using UnityEngine;

[CreateAssetMenu(fileName = "TargetDataOS", menuName = "TargetDataOS", order = 0)]
public class TargetDataOS : ScriptableObject
{
    [Tooltip("Target s gonna randomly choose 1 of these sprites")]
    [SerializeField] private Sprite[] _frontSrites;
    [SerializeField] private Sprite[] _stickSpites;
    public Sprite BackSprite;
    public float FlipDuration;
    public Vector3 EndFlipRotation;
    public float Height;
    public float SlideSpeed;
    public float SwingRange;
    public float SwingDuration;

    public Sprite RandomFrontSprite()
    {
        int idx = Random.Range(0, _frontSrites.Length);
        return _frontSrites[idx];
    }

    public Sprite RandomStickSprite()
    {
        int idx = Random.Range(0, _stickSpites.Length);
        return _stickSpites[idx];
    }
}