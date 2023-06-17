using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [SerializeField]
    private InfoPopup infoPopupPrefab;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ShowInfoPopup("Testing", "This was triggered by hitting \"P\"");
        }
    }

    public void ShowInfoPopup(string title, string body, System.Action onConfirm = null)
    {
        InfoPopup popup = Instantiate(infoPopupPrefab, this.transform);
        popup.Show(title, body, onConfirm);
    }
}
