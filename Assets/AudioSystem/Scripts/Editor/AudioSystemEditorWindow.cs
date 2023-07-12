using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.UIElements;

public class AudioSystemEditorWindow : EditorWindow
{
	public SkillBuilderSO skillBuilder;
	public VisualTreeAsset m_UXML;

	[MenuItem("PsyKaiTools/Audio System")]
	public static void Open()
	{
		var window = GetWindow<AudioSystemEditorWindow>();
	}
}