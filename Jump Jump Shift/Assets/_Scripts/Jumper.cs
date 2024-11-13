using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [Header("Inscribed")]
    public float maxSpeed = 30; // The fastest I want the jumper to move
    public float acceleration = 5; // How fast the jumper gains speed
    public float deceleration = 3; // How fast the jumper loses speed

    [Header("Dynamic")]
    public float speed = 0; // the jumpers current speed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;

        if (xAxis == 1) // If holding right, accelerate right
        {
            speed = speed + acceleration;
            if (speed > maxSpeed) 
            {
                speed = maxSpeed; 
            }
        }

        if (xAxis == -1) // If holding left, accelerate left
        {
            speed = speed - acceleration;
            if(speed < -maxSpeed)
            {
                speed = -maxSpeed;
            }
        }

        if (xAxis == 0)
        {
            speed = speed / deceleration;
        }

        pos.x = pos.x + speed * Time.deltaTime;
        transform.position = pos;
        
    }
}
