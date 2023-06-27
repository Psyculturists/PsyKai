using UnityEngine;
using UnityEngine.Events;


public class EventTrigger2D : MonoBehaviour
{
	public UnityEvent onTriggerEnter2D;
	public UnityEvent onTriggerStay2D;
	public UnityEvent onTriggerExit2D;

	[SerializeField] string collideWithTag;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collideWithTag == string.Empty)
		{
			onTriggerEnter2D?.Invoke();
		}
		else if (collision.CompareTag(collideWithTag))
		{
			onTriggerEnter2D?.Invoke();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collideWithTag == string.Empty)
		{
			onTriggerStay2D?.Invoke();
		}
		else if (collision.CompareTag(collideWithTag))
		{
			onTriggerStay2D?.Invoke();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collideWithTag == string.Empty)
		{
			onTriggerExit2D?.Invoke();
		}
		else if (collision.CompareTag(collideWithTag))
		{
			onTriggerExit2D?.Invoke();
		}
	}
}



[System.Serializable]
public class InputKeyEvent
{
	public string inputKey;
	public UnityEvent onInputKey;
	public UnityEvent onInputKeyUp;
	public UnityEvent onInputKeyDown;
}