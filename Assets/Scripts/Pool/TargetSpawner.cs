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
    [SerializeField] private VoidEventChannelSO _timesUpEvent;


    private int _numCurrentTargetAppear;
    private bool _canSpawn = true;

    private void Awake()
    {
        _pool = new ObjectPool<Target>(_targetPrefab.gameObject, null, _startQuantity);
    }

    private void OnEnable()
    {
        _startGameEvent.OnEventRaised += Spawn;
        _returnToPoolEvent.OnEventRaised += Release;
        _timesUpEvent.OnEventRaised += StopSpawning;
    }


    /// <summary>
    /// shuffle all the available spawn pos from SpawnConfig.
    /// Then get an amount of "_nbTargetEachWave" targets from pool,
    /// loop and respectively assgin them a spawn position
    /// </summary>
    private void Spawn()
    {
        if (_canSpawn)
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
    }


    /// <summary>
    /// Deactivate the target and return it to the pool.
    /// Check if all target of this spawnwave have returned? then start a new spawn wave
    /// </summary>
    /// <param name="target">the target you want it to return to pool</param>
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

    private void StopSpawning()
    {
        _canSpawn = false;
    }

    private void OnDisable()
    {
        _startGameEvent.OnEventRaised -= Spawn;
        _returnToPoolEvent.OnEventRaised -= Release;
        _timesUpEvent.OnEventRaised -= StopSpawning;
    }
}