using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.UIElements;
using UnityEngine;
using System.Collections.Generic;

public class SkillBuilderEditorWindow : EditorWindow
{
	public SkillBuilderSO skillBuilder;
	public VisualTreeAsset m_UXML;


	[MenuItem("PsyKaiTools/Skill Builder")]
	public static void Open()
	{
		var window = GetWindow<SkillBuilderEditorWindow>();
	}

	private void CreateGUI()
	{
		if (skillBuilder == null) return;

		
		m_UXML?.CloneTree(rootVisualElement);
		var serializedObjectSO = new SerializedObject(skillBuilder);
		rootVisualElement.Bind(serializedObjectSO);

		var isSkillSelected = skillBuilder.selectedSkill != null;

		var selectedSkillPanel = rootVisualElement.Q<VisualElement>("selected-skill-panel");
		selectedSkillPanel.visible = isSkillSelected;
		var deleteSkillButton = rootVisualElement.Q<Button>("delete-skill-btn");
		deleteSkillButton.clicked += DeleteSkillCallback;

		var createdSkillsList = rootVisualElement.Q<ListView>("created-skills-list");
		var listProperty = serializedObjectSO.FindProperty(nameof(skillBuilder.createdSkills));
		createdSkillsList.BindProperty(listProperty);
		createdSkillsList.showBoundCollectionSize = false;
		createdSkillsList.reorderable = false;

		createdSkillsList.makeItem = () => new Label();
		createdSkillsList.bindItem = (e, i) =>
		{
			var bindable = (BindableElement)e;
			bindable.BindProperty(listProperty.GetArrayElementAtIndex(i));
		};
		
		createdSkillsList.selectionChanged += (o) =>
		{
			if (createdSkillsList.selectedIndex < 0) return;
			var selectedSkill = skillBuilder.createdSkills[createdSkillsList.selectedIndex];
			skillBuilder.selectedSkill = selectedSkill;
			selectedSkillPanel.visible = skillBuilder.selectedSkill != null;

			var selectedObject = new SerializedObject(selectedSkill);
			//skillBuilder.skillName = skillBuilder.selectedSkill.SkillName;

			var skillNameField = rootVisualElement.Q<TextField>("skill-name-field");
			var descriptionField = rootVisualElement.Q<TextField>("description-field");
			var amountSlider = rootVisualElement.Q<SliderInt>("amount-slider");
			var attackScalingField = rootVisualElement.Q<FloatField>("attack-scaling-field");
			var selfTargetToggle = rootVisualElement.Q<Toggle>("self-target-toggle");
			var healToggle = rootVisualElement.Q<Toggle>("heal-toggle");


			//var healChoiceGroup = rootVisualElement.Q<RadioButtonGroup>("heal-choice-group");
			//var selfTargetGroup = rootVisualElement.Q<RadioButtonGroup>("self-target-choice-group");

			//skillNameField.BindProperty(serializedObjectSO.FindProperty(nameof(skillBuilder.skillName)));
			skillNameField.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.skillName)));

			skillNameField.RegisterCallback<FocusOutEvent>(RenameSkillCallback);

			descriptionField.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.description)));
			amountSlider.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.damage)));
			attackScalingField.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.attackScaling)));
			selfTargetToggle.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.isSelfTargeted)));
			healToggle.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.heals)));

			healToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
			{
				var choice = evt.newValue ? "heals" : "damage";
				amountSlider.label = string.Format("How much does it {0}?", choice);
			});
		};


		createdSkillsList.itemsChosen += (o) =>
		{
			var selectedSkill = skillBuilder.createdSkills[createdSkillsList.selectedIndex];
			EditorGUIUtility.PingObject(selectedSkill);
		};



		var createSkillButton = rootVisualElement.Q<Button>("create-skill-btn");
		createSkillButton.clicked += () =>
		{
			var skill = CreateInstance<Skill>();
			skill.name = skillBuilder.skillName;
			skill.description = skillBuilder.description;
			skill.heals = skillBuilder.isHeal;
			skill.damage = skillBuilder.damage;
			skill.attackScaling = skillBuilder.attackScaling;
			skill.isSelfTargeted = skillBuilder.isSelfTarget;
			var skillFolder = Path.Combine("Assets", "Skills");
			if (!AssetDatabase.IsValidFolder(skillFolder))
				AssetDatabase.CreateFolder("Assets","Skills");//AssetDatabase.GetAssetPath(skillBuilder);
			var path = Path.Combine(skillFolder, skill.name + ".asset");
			if (File.Exists(path))
				path = AssetDatabase.GenerateUniqueAssetPath(path);
			skill.name = Path.GetFileNameWithoutExtension(path);
			skill.skillName = skill.name;
			AssetDatabase.CreateAsset(skill, path);
			AssetDatabase.SaveAssets();
			skillBuilder.createdSkills.Add(skill);
			createdSkillsList.SetSelection(skillBuilder.createdSkills.Count - 1);
			
		};
	}

	void RenameSkillCallback(FocusOutEvent evt)
	{
		var createdSkillsList = rootVisualElement.Q<ListView>("created-skills-list");
		var selectedSkill = skillBuilder.createdSkills[createdSkillsList.selectedIndex];
		var skillPath = AssetDatabase.GetAssetPath(selectedSkill.GetInstanceID());
		AssetDatabase.RenameAsset(skillPath, selectedSkill.skillName);
		AssetDatabase.SaveAssets();
		createdSkillsList.Rebuild();
	}


	/*
	VisualElement ItemElement()
	{
		var item = new VisualElement();
		var deleteButton = new Button();
		deleteButton.text = "X";
		delet
		var label = new Label();
		item.Add(deleteButton);
		item.Add(label);
		item.style.flexDirection = FlexDirection.Row;

		return item;
	}*/

	void DeleteSkillCallback()
	{
		var selectedSkillPanel = rootVisualElement.Q<VisualElement>("selected-skill-panel");
		selectedSkillPanel.visible = false;

		var createdSkillsList = rootVisualElement.Q<ListView>("created-skills-list");
		if (createdSkillsList.selectedIndex < 0) return;
		var selectedSkill = skillBuilder.createdSkills[createdSkillsList.selectedIndex];
		var skillPath = AssetDatabase.GetAssetPath(selectedSkill.GetInstanceID());
		skillBuilder.createdSkills.RemoveAt(createdSkillsList.selectedIndex);
		createdSkillsList.RemoveAt(createdSkillsList.selectedIndex);
		AssetDatabase.DeleteAsset(skillPath);
		AssetDatabase.SaveAssets();
		createdSkillsList.SetSelection(-1);
	}

}
