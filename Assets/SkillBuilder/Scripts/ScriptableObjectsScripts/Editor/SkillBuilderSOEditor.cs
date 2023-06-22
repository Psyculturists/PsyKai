using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;


public class AssetHandler
{
	[OnOpenAsset()]
	public static bool OpenEditor(int instanceID, int line)
	{
		SkillBuilderSO obj = EditorUtility.InstanceIDToObject(instanceID) as SkillBuilderSO;
		if (obj != null)
		{
			SkillBuilderEditorWindow.Open();
			return true;
		}
		return false;
	}
}

[CustomEditor(typeof(SkillBuilderSO))]
public class SkillBuilderSOEditor : Editor
{	
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Skill Builder"))
		{
			SkillBuilderEditorWindow.Open();
		}
	}
}
