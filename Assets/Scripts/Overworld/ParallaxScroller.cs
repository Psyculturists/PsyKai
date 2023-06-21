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
    }

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private List<ParallaxLayer> parallaxLayers;

    void Start()
    {
    }

    void Update()
    {
        if (mainCamera != null)
        {
            float cameraX = mainCamera.transform.position.x;

            foreach (ParallaxLayer layer in parallaxLayers)
            {
                Vector3 newPos = layer.transform.position;

                newPos.x = cameraX * layer.distance;

                layer.transform.position = newPos;
            }
        }
    }
}
