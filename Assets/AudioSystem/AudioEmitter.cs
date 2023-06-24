using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioEmitter : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioChannel audioChannel;


	private void Awake()
	{
		SetAudioSource();
	}

	void SetAudioSource()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		audioChannel.onPlayClip += PlayClip;
		audioChannel.onPause += Pause;
		audioChannel.onResume += Resume;
	}

	private void OnDisable()
	{
		audioChannel.onPlayClip -= PlayClip;
		audioChannel.onPause -= Pause;
		audioChannel.onResume -= Resume;
	}


	public void PlayClip(AudioClipController audioClip)
	{
		//TODO: Parallel Playing
		audioSource.clip = audioClip.audioClip;
		audioSource.loop = audioClip.loopClip;
		audioSource.Play();
	}

	public void Pause()
	{
		audioSource.Pause();
	}

	public void Resume()
	{
		audioSource.UnPause();
	}
}
