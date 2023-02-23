using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyParallax : MonoBehaviour
{
    private float currentPosition;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 36)
        {
            transform.position = new Vector3(currentPosition + parallaxEffect, transform.position.y, transform.position.z);
            currentPosition = transform.position.x;
        }
        else
        {
            transform.position = new Vector3(-36, transform.position.y, transform.position.z);
            currentPosition = transform.position.x;
        }
    }
}
