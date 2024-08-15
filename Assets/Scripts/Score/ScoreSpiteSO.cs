using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSpiteSO", menuName = "ScoreSpiteSO")]
public class ScoreSpiteSO : ScriptableObject
{
    [SerializeField] private Sprite[] _sprites;


    public Sprite GetSprite(int idx)
    {
        return _sprites[idx];
    }
}