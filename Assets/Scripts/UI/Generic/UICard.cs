using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UICard : MonoBehaviour
{
    [SerializeField]
    protected Image itemImage;
    [SerializeField]
    protected TextMeshProUGUI itemNameText;
    [SerializeField]
    protected Button clickButton; //to simulate selection via UI navigation
    [SerializeField]
    protected Image selectedHighlight;

    protected ItemData data;
    public ItemData Data => data;
    protected RecipePreviewPanel previewPanel;

    protected Action<UICard> ClearAction;

    public virtual void SetupCard(ItemData inData, RecipePreviewPanel panel, Action<UICard> clearAction)
    {
        data = inData;
        itemImage.sprite = data.Icon;
        itemNameText.text = data.ItemName;

        previewPanel = panel;
        clickButton.onClick.AddListener(SelectCard);
        ClearAction = clearAction;
    }

    public virtual void SelectCard()
    {
        ClearAction?.Invoke(this);
        selectedHighlight.enabled = true;
    }

    public virtual void ClearSelect()
    {
        selectedHighlight.enabled = false;
    }
}
