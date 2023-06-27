using UnityEditor;
using UnityEditor.Callbacks;
using System;

public class AssetHandler
{
	public static event Func<int,bool> onOpenAsset;

	[OnOpenAsset()]
	static bool OnOpenAsset(int instanceID)
	{
		if (onOpenAsset != null)
		{
			return onOpenAsset.Invoke(instanceID);
		}
		return false;
	}
}
