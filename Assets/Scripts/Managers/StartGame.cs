using UnityEngine;
using DG.Tweening;

/// <summary>
/// attached to a target, will broadcast startgame event if player shoots at this target;
/// </summary>

[RequireComponent(typeof(CircleCollider2D))]
public class StartGame : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _targetSpriteRenderer;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private AudioGroupSO _onShotAudios;

    [Header("broadcast on channel:")]
    [SerializeField] private VoidEventChannelSO StartGameEvent;
    [SerializeField] private AudioEventChannelSO _sfxChannel;
    private bool _isShot = false;

    private void OnMouseDown()
    {
        OnShot();
    }

    public void OnShot()
    {
        if (!_isShot)
        {
            _isShot = true;
            StartGameEvent.RaiseEvent();
            _sfxChannel.RaiseEvent(_onShotAudios);
            FlipAndDisappear();
        }
    }
    private void FlipAndDisappear()
    {
        Vector3 endFlipRotation = new Vector3(0, 180, 0);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(endFlipRotation / 2f, 0.1f).OnComplete(
            () => _targetSpriteRenderer.sprite = _backSprite));
        sequence.Append(transform.DOLocalRotate(endFlipRotation, 0.2f).SetEase(Ease.OutBounce));
        sequence.AppendInterval(0.3f);
        sequence.Append(transform.DOLocalMoveY(transform.localPosition.y - 2, 0.3f).SetEase(Ease.InBack));
        sequence.OnComplete(() => gameObject.SetActive(false));
    }

}