using UnityEngine;
using MyObjectPool;

public class TargetSpawner : MonoBehaviour
{

    [Header("Pool variables")]
    private ObjectPool<Target> _pool;
    [SerializeField] private Target _targetPrefab;
    [SerializeField] private int _startQuantity;


    [Header("Spawn Configuration")]
    [SerializeField] private int _nbTargetEachWave;
    [SerializeField] private SpawnConfigSO _spawnConfig;


    [Header("Broadcast on channel:")]
    [SerializeField] private IntEventChannelSO _startASpawnWaveEvent;


    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO _startGameEvent;
    [SerializeField] private TargetEventChannelSO _returnToPoolEvent;

    private int _numCurrentTargetAppear;

    private void Awake()
    {
        _pool = new ObjectPool<Target>(_targetPrefab.gameObject, null, _startQuantity);
    }

    private void OnEnable()
    {
        _startGameEvent.OnEventRaised += Spawn;
        _returnToPoolEvent.OnEventRaised += Release;
    }

    private void Spawn()
    {
        _startASpawnWaveEvent.RaiseEvent(_nbTargetEachWave);
        _numCurrentTargetAppear = _nbTargetEachWave;
        _spawnConfig.ShufflePos();
        for (int i = 0; i < _nbTargetEachWave; ++i)
        {
            Target target = _pool.GetAnInstance();
            target.gameObject.SetActive(true);
            target.Spawn(_spawnConfig.spawnPositions[i]);
        }
    }

    private void Release(Target target)
    {
        target.gameObject.SetActive(false);
        _pool.ReturnToPool(target);
        _numCurrentTargetAppear--;
        if (_numCurrentTargetAppear == 0)
        {
            Spawn();
        }
    }

    private void OnDisable()
    {
        _startGameEvent.OnEventRaised -= Spawn;
        _returnToPoolEvent.OnEventRaised -= Release;
    }
}