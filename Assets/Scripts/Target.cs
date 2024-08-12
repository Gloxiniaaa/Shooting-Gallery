using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Target : MonoBehaviour, IShootable
{
    [SerializeField] private TargetDataOS _targetDataOS;
    [SerializeField] private SpriteRenderer _targetSpriteRenderer;
    [SerializeField] private SpriteRenderer _StickSpriteRenderer;
    private int _direction = 1;
    private bool _isShot;

    private void OnEnable()
    {
        transform.localRotation = quaternion.identity;
        _isShot = false;
        ChooseSprite();
        AppearAndSwing();
        Slide();
    }

    private void ChooseSprite()
    {
        _targetSpriteRenderer.sprite = _targetDataOS.RandomFrontSprite();
        _StickSpriteRenderer.sprite = _targetDataOS.RandomStickSprite();
    }

    private void OnMouseDown()
    {
        OnShot();
    }

    public void OnShot()
    {
        if (!_isShot)
        {
            _isShot = true;
            FlipAndDisappear();
        }
    }

    private void FlipAndDisappear()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(_targetDataOS.EndFlipRotation / 2f, _targetDataOS.FlipDuration * 0.4f).OnComplete(
            () => _targetSpriteRenderer.sprite = _targetDataOS.BackSprite));
        sequence.Append(transform.DOLocalRotate(_targetDataOS.EndFlipRotation, _targetDataOS.FlipDuration * 0.6f).SetEase(Ease.OutBounce));
        sequence.AppendInterval(0.5f);
        sequence.Append(transform.DOLocalMoveY(transform.localPosition.y - _targetDataOS.Height, 0.3f).SetEase(Ease.InBack));
        sequence.OnComplete(() => gameObject.SetActive(false));
    }

    private void AppearAndSwing()
    {
        transform.DOLocalMoveY(transform.localPosition.y + _targetDataOS.Height, 0.5f).SetEase(Ease.OutBack).OnComplete(
            () => transform.DOLocalMoveY(transform.localPosition.y + _targetDataOS.SwingRange, _targetDataOS.SwingDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo)
        );
    }

    private void Slide()
    {
        transform.DOLocalMoveX(transform.localPosition.x + _direction * 15, 15 / _targetDataOS.SlideSpeed).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}