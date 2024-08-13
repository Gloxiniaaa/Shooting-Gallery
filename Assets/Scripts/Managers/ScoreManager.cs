using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;
    private int _numTargetShot = 0;
    private int _scoreEachShot = 10;

    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO StartGameEvent;
    [SerializeField] private IntEventChannelSO _startASpawnWaveEvent;
    [SerializeField] private TargetEventChannelSO _shotATargetEvent;



    private void OnEnable()
    {
        _startASpawnWaveEvent.OnEventRaised += AssignNewMission;
        _shotATargetEvent.OnEventRaised += AddScore;
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


    private void OnDisable()
    {
        _startASpawnWaveEvent.OnEventRaised -= AssignNewMission;
        _shotATargetEvent.OnEventRaised -= AddScore;
    }


}