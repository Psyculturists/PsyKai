using UnityEngine;
using UnityEngine.Audio;
using System;

[CreateAssetMenu(menuName = "AudioSystem/AudioChannel", fileName = "AudioChannel")]
public class AudioChannel : ScriptableObject
{
	public AudioClipController[] clips;
	public AudioMixer audioMixer;

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
		onPlayClip?.Invoke(audioClip);
	}

	public void Pause()
	{
		onPause?.Invoke();
	}

	public void Resume()
	{
		onResume?.Invoke();
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
