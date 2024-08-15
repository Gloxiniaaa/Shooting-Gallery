using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have one Target argument.
/// Example: A game object pick up event, where the Target is the object we are interacting with.
/// </summary>

[CreateAssetMenu(menuName = "Events/Target Event Channel")]
public class TargetEventChannelSO : DescriptionBaseSO
{
	public UnityAction<Target> OnEventRaised;
	
	public void RaiseEvent(Target value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
