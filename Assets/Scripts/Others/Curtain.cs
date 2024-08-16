using DG.Tweening;
using UnityEngine;

public class Curtain : MonoBehaviour
{
    [SerializeField] private float _movementDuartion;
    private Vector3 _openPos;
    private Vector3 _closePos;


    [Header("Listen on channel: ")]
    [SerializeField] private VoidEventChannelSO _timesUpEvent;


    private void Awake()
    {
        _openPos = transform.position;
        _closePos = transform.position + Vector3.down * 10;
    }

    private void OnEnable()
    {
        _timesUpEvent.OnEventRaised += CloseCurtain;
        OpenCurtain();
    }

    [ContextMenu("Open curtain")]
    private void OpenCurtain()
    {
        transform.position = _closePos;
        transform.DOLocalMoveY(_openPos.y, _movementDuartion).SetEase(Ease.InOutBack);

        // Sequence sequence = DOTween.Sequence();
        // sequence.AppendInterval(1);
        // sequence.Append(transform.DOLocalMoveY(_openPos.y, _movementDuartion).SetEase(Ease.InOutBack));
    }

    [ContextMenu("Close curtain")]
    private void CloseCurtain()
    {
        transform.position = _openPos;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1);
        sequence.Append(transform.DOLocalMoveY(_closePos.y, _movementDuartion).SetEase(Ease.InOutBack));
    }

    private void OnDisable()
    {
        _timesUpEvent.OnEventRaised -= CloseCurtain;
    }
}