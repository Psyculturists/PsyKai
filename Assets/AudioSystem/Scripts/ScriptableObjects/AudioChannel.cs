using UnityEngine;
using UnityEngine.Audio;
using System;

[CreateAssetMenu(menuName = "AudioSystem/AudioChannel", fileName = "AudioChannel")]
public class AudioChannel : ScriptableObject
{
	public string channelName;
	public AudioClipController[] clips;
	public AudioMixerGroup audioMixerGroup;
	public AudioReceiver audioReceiver;

	public event Action<AudioClipController> onPlayClip;
	public event Action onPause;
	public event Action onResume;


	public void PlayClip(string clipName)
	{
		var audioClip = GetAudioClip(clipName);

		PlayClip(audioClip);
	}

	public void PlayClip(AudioClipController audioClip)
	{
		audioReceiver?.Play(audioClip);
	}

	public void Pause()
	{
		audioReceiver?.Pause();
	}

	public void Resume()
	{
		audioReceiver?.Resume();
	}

	public void AutoPlay(AudioClipController audioClip)
	{
		PlayClip(audioClip);
	}

	AudioClipController GetAudioClip(string clipName)
	{
		foreach (var clip in clips)
		{
			if (clip.clipName == clipName)
				return clip;
		}
		return null;
	}
}


[System.Serializable]
public class AudioReceiver
{
	public AudioSource audioSource;

	bool isPaused;
	AudioClip currentClip => audioSource?.clip;


	public AudioReceiver(AudioSource source)
	{
		audioSource = source;
	}

	public void Play(AudioClipController clipController)
	{
		if (audioSource == null) return;

		if (currentClip == clipController.audioClip)
		{
			if (audioSource.isPlaying) return;
			if (isPaused)
				Resume();
			else
				audioSource.Play();

		}

		else
		{
			audioSource.clip = clipController.audioClip;
			audioSource.loop = clipController.loopClip;
			audioSource.Play();
		}

		isPaused = false;
	}

	public void Pause()
	{
		if (audioSource == null) return;

		audioSource.Pause();
		isPaused = true;
	}

	public void Resume()
	{
		if (audioSource == null) return;

		audioSource.UnPause();
		isPaused = false;
	}

}
