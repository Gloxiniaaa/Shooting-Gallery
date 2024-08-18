using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private int _initialTimer;
    private int _countDown = 0;

    [SerializeField] private AudioGroupSO _timesUpSfx;
    [SerializeField] private AudioGroupSO _clockTickSfx;

    [Header("Broadcast on channel:")]
    [SerializeField] private VoidEventChannelSO _timesUpEvent;
    [SerializeField] private AudioEventChannelSO _sfxChannel;


    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO _startGameEvent;

    private void OnEnable()
    {
        _countDown = _initialTimer;
        _timerText.text = _countDown.ToString();
        _startGameEvent.OnEventRaised += CountDown;
    }

    private void CountDown()
    {
        InvokeRepeating(nameof(Tick), 1f, 1f);
    }

    private void Tick()
    {
        _countDown--;
        _timerText.text = _countDown.ToString();
        if (_countDown == 0)
        {
            _sfxChannel.RaiseEvent(_timesUpSfx);
            _timesUpEvent.RaiseEvent();
            CancelInvoke();
        }
        else if (_countDown == 4)
        {
            _sfxChannel.RaiseEvent(_clockTickSfx);
        }
    }

    private void OnDisable()
    {
        _startGameEvent.OnEventRaised -= CountDown;
    }
}