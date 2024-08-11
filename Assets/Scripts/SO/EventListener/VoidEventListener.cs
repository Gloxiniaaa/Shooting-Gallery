using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A flexible handler for void events in the form of a MonoBehaviour. Responses can be connected directly from the Inspector.
/// </summary>

public class VoidEventListener : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO _channel = default;

	public UnityEvent Response;

	private void OnEnable()
	{
		if (_channel != null)
			_channel.OnEventRaised += Respond;
	}

	private void OnDisable()
	{
		if (_channel != null)
			_channel.OnEventRaised -= Respond;
	}

	private void Respond()
	{
		if (Response != null)
			Response.Invoke();
	}
}