using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private int _hitTargetCount = 0;
    private int _nbTargetToShoot;

    [Header("Broadcast on channel:")]
    [SerializeField] private VoidEventChannelSO _shotAllTargetEvent;


    [Header("Listen on channel:")]
    [SerializeField] private IntEventChannelSO _startASpawnWaveEvent;
    [SerializeField] private GameObjectEventChannelSO _shotATargetEvent;
    [SerializeField] private VoidEventChannelSO _targetReachedEndEvent;


    private void OnEnable()
    {
        _startASpawnWaveEvent.OnEventRaised += AssignMission;
        _shotATargetEvent.OnEventRaised += OnShotATarget;
        _targetReachedEndEvent.OnEventRaised += DecreaseNumTargetToShoot;
    }

    private void AssignMission(int nbTargetToShoot)
    {
        _hitTargetCount = 0;
        _nbTargetToShoot = nbTargetToShoot;
    }

    private void OnShotATarget(GameObject target)
    {
        _hitTargetCount++;
        if (_hitTargetCount == _nbTargetToShoot)
        {
            _shotAllTargetEvent.RaiseEvent();
        }
    }

    private void DecreaseNumTargetToShoot()
    {
        _nbTargetToShoot--;
        if (_hitTargetCount == _nbTargetToShoot)
        {
            _shotAllTargetEvent.RaiseEvent();
        }
    }

    private void OnDisable()
    {
        _startASpawnWaveEvent.OnEventRaised -= AssignMission;
        _shotATargetEvent.OnEventRaised -= OnShotATarget;
        _targetReachedEndEvent.OnEventRaised -= DecreaseNumTargetToShoot;
    }
}