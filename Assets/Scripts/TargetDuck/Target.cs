using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Target : MonoBehaviour
{
    [SerializeField] private TargetDataOS _targetDataOS;
    [SerializeField] private SpriteRenderer _targetSpriteRenderer;
    [SerializeField] private SpriteRenderer _StickSpriteRenderer;
    private int _direction = 1;
    private bool _isShot;
    [SerializeField] private AudioGroupSO _onShotAudios;
    [Header("Broadcast on channel:")]
    [SerializeField] private TargetEventChannelSO _shotATargetEvent;
    [SerializeField] private TargetEventChannelSO _returnToPoolEvent;
    [SerializeField] private AudioEventChannelSO _sfxChannel;

    [Header("Listen on channel")]
    [SerializeField] private VoidEventChannelSO TimesUpEvent;



    private void OnEnable()
    {
        Initialize();
        TimesUpEvent.OnEventRaised += FlipAndDisappear;
    }


    /// <summary>
    /// Choose a random duck and stick sprite from "_targetDataOS".
    /// Randomly choose left or right direction
    /// </summary>
    private void Initialize()
    {
        _isShot = false;
        transform.localRotation = quaternion.identity;
        _targetSpriteRenderer.sprite = _targetDataOS.RandomFrontSprite();
        _StickSpriteRenderer.sprite = _targetDataOS.RandomStickSprite();
        _direction = UnityEngine.Random.Range(0, 2) < 1 ? 1 : -1;
        transform.localScale = new Vector2(_direction, 1) * transform.localScale.y;
    }


    /// <summary>
    /// Assign sorting order and position from "config".
    /// Then play the needed animtion.
    /// </summary>
    /// <param name="config">stores the desired pos and sorting order</param>
    public void Spawn(SpawnPosition config)
    {
        _targetSpriteRenderer.sortingOrder = config.SortingOrder;
        _StickSpriteRenderer.sortingOrder = config.SortingOrder;
        transform.position = new Vector3(config.X, config.Y, 0);
        AppearAndSwing();
        Slide();
    }

    // private void OnMouseDown()
    // {
    //     OnShot();
    // }


    /// <summary>
    /// if has not been shot, then trigger "_shotATargetEvent", play shot audio, then play the disappear animation
    /// </summary>
    public void OnShot()
    {
        if (!_isShot)
        {
            _isShot = true;
            _shotATargetEvent.RaiseEvent(this);
            _sfxChannel.RaiseEvent(_onShotAudios);
            FlipAndDisappear();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isShot && other.CompareTag("Finish"))
        {
            _returnToPoolEvent.RaiseEvent(this);
        }
    }


    /// <summary>
    /// flip to the back sprite, then dive down, disappear into the water
    /// </summary>
    private void FlipAndDisappear()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(_targetDataOS.EndFlipRotation / 2f, _targetDataOS.FlipDuration * 0.4f).OnComplete(
            () => _targetSpriteRenderer.sprite = _targetDataOS.BackSprite));
        sequence.Append(transform.DOLocalRotate(_targetDataOS.EndFlipRotation, _targetDataOS.FlipDuration * 0.6f).SetEase(Ease.OutBounce));
        sequence.AppendInterval(0.3f);
        sequence.Append(transform.DOLocalMoveY(transform.localPosition.y - _targetDataOS.Height, 0.3f).SetEase(Ease.InBack));
        sequence.OnComplete(() => _returnToPoolEvent.RaiseEvent(this));
    }


    /// <summary>
    /// appear up from under the water and swing up and down
    /// </summary>
    private void AppearAndSwing()
    {
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            //up first
            transform.DOLocalMoveY(transform.localPosition.y + _targetDataOS.Height - _targetDataOS.SwingRange / 2, 0.5f).SetEase(Ease.OutBack).OnComplete(
                () => transform.DOLocalMoveY(transform.localPosition.y + _targetDataOS.SwingRange, _targetDataOS.SwingDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo)
            );
        }
        else
        {
            //down first
            transform.DOLocalMoveY(transform.localPosition.y + _targetDataOS.Height + _targetDataOS.SwingRange / 2, 0.5f).SetEase(Ease.OutBack).OnComplete(
                () => transform.DOLocalMoveY(transform.localPosition.y - _targetDataOS.SwingRange, _targetDataOS.SwingDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo)
            );
        }
    }

    /// <summary>
    /// slide horizontally in a specified direction
    /// </summary>
    private void Slide()
    {
        transform.DOLocalMoveX(transform.localPosition.x + _direction * 15, 15 / _targetDataOS.SlideSpeed).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        transform.DOKill();
        TimesUpEvent.OnEventRaised -= FlipAndDisappear;
    }
}