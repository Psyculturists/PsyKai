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

    // Start is called before the first frame update
    void Start()
    {
        confirmButton.onClick.AddListener(Close);
    }

    public void Show(string title, string body)
    {
        titleText.text = title;
        bodyText.text = body;
    }

    private void Close()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
