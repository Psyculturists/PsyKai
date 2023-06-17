using UnityEngine;
using UnityEditor;
using System.IO;


[CustomEditor(typeof(SkillBuilderSO))]
public class SkillBuilderSOEditor : Editor
{

	public override void OnInspectorGUI()
	{
		var _target = (SkillBuilderSO)target;
		_target.skillName = EditorGUILayout.TextField("Skill Name", _target.skillName);
		GUILayout.Label("Description");
		_target.description = EditorGUILayout.TextArea(_target.description, GUILayout.MinHeight(40));


		if (GUILayout.Button("Create Skill"))
		{
			var skill = CreateInstance<Skill>();
			skill.name = _target.skillName;
			skill.SkillName = skill.name;
			var assetPath = AssetDatabase.GetAssetPath(target);
			var path = Path.Combine(Path.GetDirectoryName(assetPath), skill.name+".asset");
			if (File.Exists(path))
				path = AssetDatabase.GenerateUniqueAssetPath(path);
			AssetDatabase.CreateAsset(skill, path);
			AssetDatabase.SaveAssets();
		}
	}
}
