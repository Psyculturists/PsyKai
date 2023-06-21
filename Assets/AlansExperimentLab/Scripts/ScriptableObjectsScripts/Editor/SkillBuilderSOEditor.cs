using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.IO;


[CustomEditor(typeof(SkillBuilderSO))]
public class SkillBuilderSOEditor : Editor
{

	public VisualTreeAsset m_UXML;

	/*
	public override VisualElement CreateInspectorGUI()
	{
		var root = new VisualElement();
		m_UXML?.CloneTree(root);


		var skillNameField = root.Q<TextField>("skill-name-field");
		var descriptionField = root.Q<TextField>("description-field");
		var healChoiceGroup = root.Q<RadioButtonGroup>("heal-choice-group");
		var amountSlider = root.Q<SliderInt>("amount-slider");
		var selfTargetGroup = root.Q<RadioButtonGroup>("self-target-choice-group");

		healChoiceGroup.RegisterCallback<ChangeEvent<int>>((evt) =>
		{
			var choice = healChoiceGroup.value == 0 ? "heals" : "damage";
			amountSlider.label = string.Format("How much does it {0}?", choice);
		});

		var createSkillButton = root.Q<Button>("create-skill-btn");
		createSkillButton.clicked += () =>
		{
			var skill = CreateInstance<Skill>();
			skill.name = skillNameField.value;
			skill.SkillName = skill.name;
			skill.Description = descriptionField.value;
			skill.Heals = healChoiceGroup.value == 0;
			skill.Damage = amountSlider.value;
			skill.IsSelfTargeted = selfTargetGroup.value == 0;
			var assetPath = AssetDatabase.GetAssetPath(target);
			var path = Path.Combine(Path.GetDirectoryName(assetPath), skill.name + ".asset");
			if (File.Exists(path))
				path = AssetDatabase.GenerateUniqueAssetPath(path);
			AssetDatabase.CreateAsset(skill, path);
			AssetDatabase.SaveAssets();
		};


		return root;
	}*/

	
	public override void OnInspectorGUI()
	{
		GUILayout.Button("Open Skill Builder");
		/*
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
		*/
	}
}
