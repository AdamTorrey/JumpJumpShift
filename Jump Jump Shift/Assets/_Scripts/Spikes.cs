using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Spikes : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedWith = collision.gameObject; // If the jumper collides with this
        if (collidedWith.CompareTag("Jumper"))
        {
            collision.collider.GetComponent<Death>().DeathAnim();
        }
    }
}
