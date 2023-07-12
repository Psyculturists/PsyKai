using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkillBuilderSO))]
public class SkillBuilderSOEditor : Editor
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
		SkillBuilderSO obj = EditorUtility.InstanceIDToObject(instanceID) as SkillBuilderSO;
		if (obj != null)
		{
			SkillBuilderEditorWindow.Open();
			return true;
		}
		return false;
	}


	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Skill Builder"))
		{
			SkillBuilderEditorWindow.Open();
		}
	}
}
