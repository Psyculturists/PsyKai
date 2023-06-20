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
	public static void CreateMenu()
	{
		var window = GetWindow<SkillBuilderEditorWindow>();
	}

	private void CreateGUI()
	{
		if (skillBuilder == null) return;

		/*
		var scrolView = new ScrollView() { viewDataKey = "WindowScrollView" };
		scrolView.Add(new InspectorElement(skillBuilder));
		rootVisualElement.Add(scrolView);
		*/
		m_UXML?.CloneTree(rootVisualElement);
		var serializedObjectSO = new SerializedObject(skillBuilder);
		rootVisualElement.Bind(serializedObjectSO);

		var isSkillSelected = skillBuilder.selectedSkill != null;

		var selectedSkillPanel = rootVisualElement.Q<VisualElement>("selected-skill-panel");
		selectedSkillPanel.visible = isSkillSelected;


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
			var selectedSkill = skillBuilder.createdSkills[createdSkillsList.selectedIndex];
			skillBuilder.selectedSkill = selectedSkill;
			selectedSkillPanel.visible = skillBuilder.selectedSkill != null;

			var selectedObject = new SerializedObject(selectedSkill);
			//skillBuilder.skillName = skillBuilder.selectedSkill.SkillName;

			var skillNameField = rootVisualElement.Q<TextField>("skill-name-field");
			var descriptionField = rootVisualElement.Q<TextField>("description-field");
			var amountSlider = rootVisualElement.Q<SliderInt>("amount-slider");
			var selfTargetToggle = rootVisualElement.Q<Toggle>("self-target-toggle");
			var healToggle = rootVisualElement.Q<Toggle>("heal-toggle");
			//var healChoiceGroup = rootVisualElement.Q<RadioButtonGroup>("heal-choice-group");
			//var selfTargetGroup = rootVisualElement.Q<RadioButtonGroup>("self-target-choice-group");

			//skillNameField.BindProperty(serializedObjectSO.FindProperty(nameof(skillBuilder.skillName)));
			skillNameField.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.skillName)));
			skillNameField.RegisterCallback<NavigationSubmitEvent>((evt) =>
			{
				var skillPath = AssetDatabase.GetAssetPath(selectedSkill.GetInstanceID());
				AssetDatabase.RenameAsset(skillPath, selectedSkill.skillName);
				AssetDatabase.SaveAssets();
			});

			descriptionField.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.description)));
			amountSlider.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.damage)));
			selfTargetToggle.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.isSelfTargeted)));
			healToggle.BindProperty(selectedObject.FindProperty(nameof(selectedSkill.heals)));

			healToggle.RegisterCallback<ChangeEvent<bool>>((evt) =>
			{
				var choice = evt.newValue ? "heals" : "damage";
				amountSlider.label = string.Format("How much does it {0}?", choice);
			});
		};



		var createSkillButton = rootVisualElement.Q<Button>("create-skill-btn");
		createSkillButton.clicked += () =>
		{
			var skill = CreateInstance<Skill>();
			skill.name = skillBuilder.skillName;
			skill.description = skillBuilder.description;
			skill.heals = skillBuilder.isHeal;
			skill.damage = skillBuilder.damage;
			skill.isSelfTargeted = skillBuilder.isSelfTarget;
			var assetPath = AssetDatabase.GetAssetPath(skillBuilder);
			var path = Path.Combine(Path.GetDirectoryName(assetPath), skill.name + ".asset");
			if (File.Exists(path))
				path = AssetDatabase.GenerateUniqueAssetPath(path);
			skill.name = Path.GetFileNameWithoutExtension(path);
			skill.skillName = skill.name;
			AssetDatabase.CreateAsset(skill, path);
			AssetDatabase.SaveAssets();
			skillBuilder.createdSkills.Add(skill);
			
		};
	}
}