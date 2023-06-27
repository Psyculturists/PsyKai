using UnityEngine;

[CreateAssetMenu(menuName = "AudioSystem/VolumeSettings", fileName = "VolumeSetting")]
public class VolumeSettings : ScriptableObject
{
	public AudioChannel masterChannel;
	public string volumeName;

	public void ChangeVolume(float value)
	{
		masterChannel.audioMixerGroup.audioMixer.SetFloat(volumeName, Mathf.Log10(value) * 20);
	}
}