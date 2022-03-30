using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] GameObject Camera;
    public float x_ScrollSpeed = 100f;
    public float y_ScrollSpeed = 100f;

    void Start()
    {
        x_ScrollSpeed /= 100;
        y_ScrollSpeed /= 100;
    }
    void Update()
    {
        transform.position = new Vector3(Camera.transform.position.x * x_ScrollSpeed, Camera.transform.position.y * y_ScrollSpeed, 0);
    }
}