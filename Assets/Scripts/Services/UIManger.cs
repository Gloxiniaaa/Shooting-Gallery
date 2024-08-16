using UnityEngine;

public class UIManger : MonoBehaviour
{
    [SerializeField] private GameObject _ingameCanvas;


    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO _startGameEvent;
    [SerializeField] private VoidEventChannelSO _timesUpEvent;

    private void Awake()
    {
        _ingameCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        _startGameEvent.OnEventRaised += ShowCanvas;
        _timesUpEvent.OnEventRaised += HideCanvas;
    }

    private void ShowCanvas()
    {
        _ingameCanvas.SetActive(true);
    }

    private void HideCanvas()
    {
        _ingameCanvas.SetActive(false);
    }

    private void OnDisable()
    {
        _startGameEvent.OnEventRaised -= ShowCanvas;
        _timesUpEvent.OnEventRaised -= HideCanvas;
    }
}