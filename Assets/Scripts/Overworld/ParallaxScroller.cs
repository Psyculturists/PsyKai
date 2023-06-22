using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [System.Serializable]
    public struct ParallaxLayer
    {
        [SerializeField]
        public Transform transform;
        [SerializeField]
        [Range(0, 1)]
        public float distance;
        [HideInInspector]
        public float offset;
    }

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private ParallaxLayer[] parallaxLayers;

    void Start()
    {
        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            float cameraX = mainCamera.transform.position.x;

            parallaxLayers[i].offset = cameraX - parallaxLayers[i].transform.position.x; 
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            float cameraX = mainCamera.transform.position.x;

            foreach (ParallaxLayer layer in parallaxLayers)
            {
                Vector3 newPos = layer.transform.position;

                newPos.x = cameraX * layer.distance - layer.offset;

                layer.transform.position = newPos;
            }
        }
    }
}
