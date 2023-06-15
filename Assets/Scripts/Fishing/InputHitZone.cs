using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHitZone : MonoBehaviour
{
    [SerializeField]
    private Image hitMarker;
    [SerializeField]
    private Color hitColour;
    [SerializeField]
    private Color missingColour;

    [SerializeField]
    private float distanceCheckValue = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHitState(false);
    }

    

    public void UpdateHitState(bool hitting)
    {
        hitMarker.color = hitting ? hitColour : missingColour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
