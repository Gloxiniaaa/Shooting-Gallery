using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraShakeSO _cameraShakeConfig;
    [Header("Listen on channel:")]
    [SerializeField] private TargetEventChannelSO _shotATargetEvent;

    
    private Vector3 _originalPos;

    private void OnEnable()
    {
        _originalPos = _camera.transform.position;
        _shotATargetEvent.OnEventRaised += Shake;
    }

    private void Shake(Target target)
    {
        _camera.transform.position = _originalPos;
        _camera.DOShakePosition(_cameraShakeConfig.Duration, _cameraShakeConfig.Strength, _cameraShakeConfig.Vibrato, _cameraShakeConfig.Randomness);
    }

    private void OnDisable()
    {
        _shotATargetEvent.OnEventRaised -= Shake;
    }

}