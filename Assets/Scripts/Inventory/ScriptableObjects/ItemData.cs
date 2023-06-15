using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [SerializeField]
    private string itemName;
    public string ItemName => itemName;
    [SerializeField]
    private string uniqueID;
    public string UniqueID => uniqueID;
    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;
}
