using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChanging : MonoBehaviour
{

    public Rigidbody2D RB;

    [Header("Inscribed")]
    public int stateWhenActive = 1;

    [Header("Dynamic")]
    public int currentState = 1;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) // Press 1 to change state to 1
        {
            currentState = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) // Press 2 to change state to 2
        { 
            currentState = 2; 
        }

        if (stateWhenActive == currentState) // If the current state is the one that matches the obstacle, then the obstacle is active
        {
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1f;
            mat.color = c;
            RB.simulated = true;
        }

        if (stateWhenActive != currentState)// If the states don't match, it is not active
        {
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.3f;
            mat.color = c;
            RB.simulated = false;

        }
    }
}
