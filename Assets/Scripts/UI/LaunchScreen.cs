using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchScreen : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private NavigationBar navBar;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start launch screen");
        startButton.onClick.AddListener(CloseLaunchScreen);
    }

    private void CloseLaunchScreen()
    {
        Debug.Log("close launch");
        this.gameObject.SetActive(false);
        navBar.OpenHub();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
