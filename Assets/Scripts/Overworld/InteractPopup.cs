using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public Vector3 offset;

    private Camera cam;
    private bool playerIsClose;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        popupPanel.SetActive(playerIsClose);

        FollowCharacter();
    }

    private void FollowCharacter()
    {
        Vector3 pos = cam.WorldToScreenPoint(transform.position + offset);
        if (popupPanel.transform.position != pos)
        {
            popupPanel.transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }
}
