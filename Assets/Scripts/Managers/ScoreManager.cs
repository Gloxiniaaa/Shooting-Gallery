using TMPro;
using UnityEngine;
using MyObjectPool;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    private int _score;
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

    private void AddScore(Target target)
    {
        _numTargetShot++;
        _score += _scoreEachShot * _numTargetShot;
        _scoreText.text = _score.ToString();
    }

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