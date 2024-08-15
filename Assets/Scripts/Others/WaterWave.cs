using DG.Tweening;
using UnityEngine;

public class WaterWave : MonoBehaviour
{
    [SerializeField] private float _duration;
    private float _range = 1f;

    private void Start()
    {
        Wave();
    }

    private void Wave()
    {
        float end = transform.position.x + _range;
        transform.DOLocalMoveX(end, _duration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }
}