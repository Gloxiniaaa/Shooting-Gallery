using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private int _initialTimer;
    private int _countDown = 0;

    [SerializeField] private AudioGroupSO _TimesUpSfx;

    [Header("Broadcast on channel:")]
    [SerializeField] private VoidEventChannelSO _timesUpEvent;
    [SerializeField] private AudioEventChannelSO _sfxChannel;


    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO _startGameEvent;

    private void OnEnable()
    {
        _countDown = _initialTimer;
        TimerText.text = _countDown.ToString();
        _startGameEvent.OnEventRaised += CountDown;
    }

    private void CountDown()
    {
        InvokeRepeating(nameof(Tick), 1f, 1f);
    }

    private void Tick()
    {
        _countDown--;
        TimerText.text = _countDown.ToString();
        if (_countDown == 0)
        {
            _sfxChannel.RaiseEvent(_TimesUpSfx);
            _timesUpEvent.RaiseEvent();
            CancelInvoke();
        }
    }

    private void OnDisable()
    {
        _startGameEvent.OnEventRaised -= CountDown;
    }
}