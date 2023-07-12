using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioSystem))]
public class AudioSystemEditor : Editor
{

	private void OnEnable()
	{
		AssetHandler.onOpenAsset += OpenAsset;
	}

	private void OnDisable()
	{
		AssetHandler.onOpenAsset -= OpenAsset;
	}

	static bool OpenAsset(int instanceID)
	{
		AudioSystem obj = EditorUtility.InstanceIDToObject(instanceID) as AudioSystem;
		if (obj != null)
		{
			AudioSystemEditorWindow.Open();
			return true;
		}
		return false;
	}

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Audio System"))
		{
			AudioSystemEditorWindow.Open();
		}
	}
}
