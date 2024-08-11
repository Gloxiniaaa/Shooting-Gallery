using UnityEngine;

/// <summary>
/// attached to a target, will broadcast startgame event if player shoots at this target;
/// </summary>

[RequireComponent(typeof(CircleCollider2D))]
public class StartGame : MonoBehaviour
{
    [Header("broadcast on channel:")]
    [SerializeField] private VoidEventChannelSO StartGameEvent;

    private void OnMouseDown()
    {
        StartGameEvent.RaiseEvent();
    }
}