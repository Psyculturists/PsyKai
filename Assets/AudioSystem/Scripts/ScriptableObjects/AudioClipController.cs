using UnityEngine;

[CreateAssetMenu(menuName = "AudioSystem/AudioClipController", fileName = "AudioClipController")]
public class AudioClipController : ScriptableObject
{
	public string clipName;
	public AudioClip audioClip;
	public AudioChannel audioChannel;
	public bool loopClip;

	public void Play()
	{
		audioChannel?.PlayClip(this);
	}

	public void Play(AudioChannel audioChannel)
	{
		audioChannel.PlayClip(this);
	}
}
