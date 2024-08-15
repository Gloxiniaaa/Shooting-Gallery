using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private int _initialTimer;
    private int _countDown = 0;

    [Header("Broadcast on channel:")]
    [SerializeField] private VoidEventChannelSO TimesUpEvent;


    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO StartGameEvent;

    private void OnEnable()
    {
        _countDown = _initialTimer;
        TimerText.text = _countDown.ToString();
        StartGameEvent.OnEventRaised += CountDown;
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
            TimesUpEvent.RaiseEvent();
            CancelInvoke();
        }
    }

    private void OnDisable()
    {
        StartGameEvent.OnEventRaised -= CountDown;
    }
}