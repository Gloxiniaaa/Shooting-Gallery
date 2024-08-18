using TMPro;
using UnityEngine;
using MyObjectPool;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static int _score { get; private set; }
    private int _numTargetShot = 0;
    private int _scoreEachShot = 10;
    [SerializeField] private TextMeshProUGUI _scoreText;


    [Header("Plus score effect")]
    [SerializeField] private PlusScore _plusScorePrefab;
    [SerializeField] private AudioGroupSO _plusScoreSfx;
    private ObjectPool<PlusScore> _plusScoreVfxPool;


    [Header("Broadcast on channel:")]
    [SerializeField] private AudioEventChannelSO _sfxChannel;


    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO _startGameEvent;
    [SerializeField] private IntEventChannelSO _startASpawnWaveEvent;
    [SerializeField] private TargetEventChannelSO _shotATargetEvent;


    private void Awake()
    {
        _plusScoreVfxPool = new ObjectPool<PlusScore>(_plusScorePrefab.gameObject, null, 7);
    }

    private void OnEnable()
    {
        _startASpawnWaveEvent.OnEventRaised += AssignNewMission;
        _shotATargetEvent.OnEventRaised += AddScore;
        _shotATargetEvent.OnEventRaised += SpawnPlusScoreEffect;
    }

    private void AssignNewMission(int nbTargetToShoot)
    {
        _numTargetShot = 0;
    }


    /// <summary>
    ///  Add up to the current score an amount of _scoreEachShot multiplied by the number of targets that have been shot.
    /// </summary>
    /// <param name="target">just to match the callback, has no use in this function</param>
    private void AddScore(Target target)
    {
        _numTargetShot++;
        _score += _scoreEachShot * _numTargetShot;
        _scoreText.text = _score.ToString();
    }


    /// <summary>
    /// Get plusScoreEffect from pool and spawn it at the target 's position.
    /// Also play the sfx
    /// </summary>
    /// <param name="target"></param>
    private void SpawnPlusScoreEffect(Target target)
    {
        PlaySFX();
        PlusScore plusSore = _plusScoreVfxPool.GetAnInstance();
        plusSore.transform.position = target.transform.position + Vector3.up * 1.1f;
        plusSore.SetScore(_scoreEachShot * _numTargetShot);
        plusSore.gameObject.SetActive(true);
        StartCoroutine(ReturnVFXToPool(plusSore));
    }

    private void PlaySFX()
    {
        _sfxChannel.RaiseEvent(_plusScoreSfx);
    }

    private IEnumerator ReturnVFXToPool(PlusScore plusScore)
    {
        yield return new WaitForSeconds(1.5f);
        _plusScoreVfxPool.ReturnToPool(plusScore);
    }

    private void OnDisable()
    {
        _startASpawnWaveEvent.OnEventRaised -= AssignNewMission;
        _shotATargetEvent.OnEventRaised -= AddScore;
        _shotATargetEvent.OnEventRaised -= SpawnPlusScoreEffect;
    }


}