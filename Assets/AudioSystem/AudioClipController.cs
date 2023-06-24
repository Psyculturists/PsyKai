using UnityEngine;

[CreateAssetMenu(menuName = "AudioSystem/AudioClipController", fileName = "AudioClipController")]
public class AudioClipController : ScriptableObject
{
	public string clipName;
	public AudioClip audioClip;
	public bool loopClip;

	[SerializeField]
	AudioChannel audioChannel;

	public void Play()
	{
		audioChannel?.PlayClip(this);
	}

	public void Play(AudioChannel audioChannel)
	{
		audioChannel.PlayClip(this);
	}
}
