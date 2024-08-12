using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Target _targetPrefab;
    private ObjectPool<Target> _pool;


    [Header("Spawn Configuration")]
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxSize;
    [SerializeField] private int _nbTargetEachWave;
    [SerializeField] private SpawnConfigSO _spawnConfig;


    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO _startGameEvent;

    private void Awake()
    {
        _pool = new ObjectPool<Target>(OnCreateInstance, OnGetFromPool, OnReturnToPool, OnDestroyPooledObject, true, _defaultCapacity, _maxSize);
    }

    private void OnEnable()
    {
        _startGameEvent.OnEventRaised += Spawn;
    }

    private void Spawn()
    {
        _spawnConfig.ShufflePos();
        for (int i = 0; i < _nbTargetEachWave; ++i)
        {
            Target target = _pool.Get();
            target.Spawn(_spawnConfig.spawnPositions[i]);
            StartCoroutine(Release(target));
        }
    }

    private IEnumerator Release(Target target)
    {
        yield return new WaitForSeconds(_spawnConfig.TargetLifeTime);
        _pool.Release(target);
    }


    #region Object Pool callback
    private Target OnCreateInstance()
    {
        return Instantiate(_targetPrefab);
    }

    private void OnGetFromPool(Target target)
    {
        target.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Target target)
    {
        target.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Target target)
    {
        Destroy(target.gameObject);
    }
    private void OnDisable()
    {
        _startGameEvent.OnEventRaised -= Spawn;
    }
    #endregion
}