using DG.Tweening;
using UnityEngine;

public class PlusScore : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _firstDigit;
    [SerializeField] private ScoreSpiteSO _scoreSprites;



    private void OnEnable()
    {
        PlayEffect();
    }

    private void PlayEffect()
    {
        transform.localScale = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, Random.Range(-0.05f, 0.05f), transform.rotation.w);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1, 0.3f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.5f);
        sequence.Append(transform.DOScale(0, 0.2f).SetEase(Ease.InBack));
        sequence.OnComplete(() => gameObject.SetActive(false));
    }

    /// <summary>
    /// set the sprite to appear that matches the desired score
    /// </summary>
    /// <param name="score"> alows 10, 20, 30,... 90</param>
    public void SetScore(int score)
    {
        int firstDigit = score / 10;
        _firstDigit.sprite = _scoreSprites.GetSprite(firstDigit);
    }
}