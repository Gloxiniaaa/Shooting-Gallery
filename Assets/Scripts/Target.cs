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
    [SerializeField] private AudioGroupSO _onShotAudios;
    [Header("Broadcast on channel:")]
    [SerializeField] private GameObjectEventChannelSO _shotATargetEvent;
    [SerializeField] private VoidEventChannelSO _targetReachedEndEvent;
    [SerializeField] private AudioEventChannelSO _sfxChannel;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _isShot = false;
        transform.localRotation = quaternion.identity;
        _targetSpriteRenderer.sprite = _targetDataOS.RandomFrontSprite();
        _StickSpriteRenderer.sprite = _targetDataOS.RandomStickSprite();
        _direction = UnityEngine.Random.Range(0, 2) < 1 ? 1 : -1;
        transform.localScale = new Vector2(_direction, 1) * transform.localScale.y;
    }

    public void Spawn(SpawnPosition config)
    {
        _targetSpriteRenderer.sortingOrder = config.SortingOrder;
        _StickSpriteRenderer.sortingOrder = config.SortingOrder;
        transform.position = new Vector3(config.X, config.Y, 0);
        AppearAndSwing();
        Slide();
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
            _shotATargetEvent.RaiseEvent(this.gameObject);
            _sfxChannel.RaiseEvent(_onShotAudios);
            FlipAndDisappear();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            _targetReachedEndEvent.RaiseEvent();
            gameObject.SetActive(false);
        }
    }

    private void FlipAndDisappear()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalRotate(_targetDataOS.EndFlipRotation / 2f, _targetDataOS.FlipDuration * 0.4f).OnComplete(
            () => _targetSpriteRenderer.sprite = _targetDataOS.BackSprite));
        sequence.Append(transform.DOLocalRotate(_targetDataOS.EndFlipRotation, _targetDataOS.FlipDuration * 0.6f).SetEase(Ease.OutBounce));
        sequence.AppendInterval(0.3f);
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