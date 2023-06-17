using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI bodyText;
    [SerializeField]
    private Button confirmButton;

    private System.Action onConfirmAction;

    // Start is called before the first frame update
    void Start()
    {
        confirmButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        onConfirmAction = null;
    }

    public void Show(string title, string body, System.Action onConfirm = null)
    {
        titleText.text = title;
        bodyText.text = body;
        onConfirmAction = onConfirm;
    }

    private void Close()
    {
        onConfirmAction?.Invoke();
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
