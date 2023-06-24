using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveTop : MonoBehaviour
{
    public float yHeight;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var position = gameObject.transform.position;
        position.y = yHeight;
        gameObject.transform.position = position;
    }
}
