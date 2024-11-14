using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [Header("Inscribed")]
    public float maxSpeed = 20; // The fastest I want the jumper to move
    public float acceleration = 2; // How fast the jumper gains speed
    public float deceleration = 1.75f; // How fast the jumper loses speed

    [Header("Dynamic")]
    public float horizonSpeed = 0; // the jumpers current speed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;

        horizonSpeed = horizonSpeed + (acceleration * xAxis);

        if (horizonSpeed > maxSpeed)
        {
            horizonSpeed = maxSpeed;
        }

        if (horizonSpeed < -maxSpeed)
        {
            horizonSpeed = -maxSpeed;
        }

        if (xAxis == 0)
        {
            horizonSpeed = horizonSpeed / deceleration;
        }

        pos.x = pos.x + horizonSpeed * Time.deltaTime;
        transform.position = pos;
        
    }
}
