using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManger : MonoBehaviour
{
    [SerializeField] private GameObject _ingameCanvas;
    [SerializeField] private GameObject _endgameBoard;
    [SerializeField] private TextMeshProUGUI _finalScore;

    [Header("Listen on channel:")]
    [SerializeField] private VoidEventChannelSO _startGameEvent;
    [SerializeField] private VoidEventChannelSO _timesUpEvent;

    private void Awake()
    {
        _ingameCanvas.SetActive(false);
        _endgameBoard.SetActive(false);
    }

    private void OnEnable()
    {
        _startGameEvent.OnEventRaised += ShowCanvas;
        _timesUpEvent.OnEventRaised += HideCanvas;
        _timesUpEvent.OnEventRaised += ShowFinalScore;
    }

    private void ShowCanvas()
    {
        _ingameCanvas.SetActive(true);
    }

    private void HideCanvas()
    {
        _ingameCanvas.SetActive(false);
    }

    
    private void ShowFinalScore()
    {
        _endgameBoard.transform.localScale = Vector2.zero;
        _finalScore.text = ScoreManager.Score.ToString();
        _endgameBoard.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.Append(_endgameBoard.transform.DOScale(Vector2.one, 0.5f));
    }

    public void PressReplayBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        _startGameEvent.OnEventRaised -= ShowCanvas;
        _timesUpEvent.OnEventRaised -= HideCanvas;
        _timesUpEvent.OnEventRaised -= ShowFinalScore;
    }
}