using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 2f;
    public float xoffset = 5f;

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x - xoffset, target.position.y, 0f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
