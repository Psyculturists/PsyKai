using UnityEngine;

public class AudioManager : MonoBehaviour
{
	static AudioManager current;

	public AudioSystem audioSystem;


	private void Awake()
	{
		if (current != null && current != this)
		{
			Destroy(gameObject);
			return;
		}

		current = this;
		DontDestroyOnLoad(gameObject);
		SetChannels();
	}


	void SetChannels()
	{
		foreach(var channel in audioSystem.channels)
		{
			var audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.outputAudioMixerGroup = channel.audioMixerGroup;
			channel.audioReceiver = new AudioReceiver(audioSource);
		}
	}
}
