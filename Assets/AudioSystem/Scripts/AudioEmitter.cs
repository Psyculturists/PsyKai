using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
	public AudioClipController clipController;
	public bool autoPlay;
	public bool is3DAudio;

	public AudioChannel audioChannel3D;

	AudioChannel audioChannel => is3DAudio ? audioChannel3D : clipController.audioChannel;


	private void Awake()
	{
		if (is3DAudio)
		{
			SetAudio3D();
		}
	}

	public void SetAudio3D()
	{
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		if (audioSource == null)
			audioSource = gameObject.AddComponent<AudioSource>();

		audioChannel3D.audioMixerGroup = clipController.audioChannel.audioMixerGroup;

		audioSource.outputAudioMixerGroup = audioChannel3D.audioMixerGroup;
		audioChannel3D.audioReceiver = new AudioReceiver(audioSource);
	}

	private void Start()
	{
		if (autoPlay)
		{
			AutoPlay();
		}
	}

	void AutoPlay()
	{
		audioChannel.AutoPlay(clipController);
	}


	public void PlayClip()
	{
		audioChannel.PlayClip(clipController);
	}

	public void Pause()
	{
		audioChannel.Pause();
	}

	public void Resume()
	{
		audioChannel.Resume();
	}
}
