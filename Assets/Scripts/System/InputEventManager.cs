using UnityEngine;

public class InputEventManager : MonoBehaviour
{
	public InputKeyEvent[] inputKeyEvents;


	private void Update()
	{
		for (int i = 0; i < inputKeyEvents.Length; i++)
		{
			var inputKey = inputKeyEvents[i].inputKey;
			if (Input.GetButton(inputKey))
			{
				inputKeyEvents[i].onInputKey?.Invoke();
			}
			if (Input.GetButtonDown(inputKey))
			{
				inputKeyEvents[i].onInputKeyDown?.Invoke();
			}
			if (Input.GetButtonUp(inputKey))
			{
				inputKeyEvents[i].onInputKeyUp?.Invoke();
			}
		}
	}
}
