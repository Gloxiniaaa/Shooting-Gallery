using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Target : MonoBehaviour, IShootable
{
    [SerializeField] private TargetDataOS _targetDataOS;
    [SerializeField] private SpriteRenderer _targetSpriteRenderer;

    private void OnMouseDown()
    {
        OnShot();
    }

    public void OnShot()
    {
        Flip();
    }

    private void Flip()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(_targetDataOS.EndFlipRotation / 2, _targetDataOS.FlipDuration / 3).OnComplete(
            () => _targetSpriteRenderer.sprite = _targetDataOS.BackSprite));
        sequence.Append(transform.DOLocalRotate(_targetDataOS.EndFlipRotation, _targetDataOS.FlipDuration * 2 / 3).SetEase(Ease.OutBounce));
    }
}