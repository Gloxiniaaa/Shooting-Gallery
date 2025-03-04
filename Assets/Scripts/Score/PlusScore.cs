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


    /// <summary>
    /// Play the appear and disappear animation.
    /// Deactivate itself when finish.
    /// </summary>
    private void PlayEffect()
    {
        transform.localScale = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, Random.Range(-0.07f, 0.07f), transform.rotation.w);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1, 0.3f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.3f);
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