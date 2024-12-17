using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public Jumper Jumper;

    [Header("Animations")]
    [SerializeField] private Animator jumpAnim;

    [Header("Dynamic")]
    public Vector3 jumperSpawn; // Spawn location of the jumper

    void Awake()
    {
        jumperSpawn = transform.position;
    }

    public void DeathAnim()
    {
        GameObject jumper = GameObject.Find("Jumper");
        jumpAnim.Play("Dying", 0, 0);
        jumper.GetComponent<Jumper>().Dying();
    }

    public void Respawn()
    {
        GameObject jumper = GameObject.Find("Jumper");
        jumper.transform.position = jumperSpawn;
        jumpAnim.Play("Respawning", 0, 0);
        Invoke("Respawned", .75f);
    }

    void Respawned()
    {
        GameObject jumper = GameObject.Find("Jumper");
        jumper.GetComponent<Jumper>().Respawning();
    }

    public void LevelComplete()
    {
        GameObject jumper = GameObject.Find("Jumper");
        jumper.GetComponent<Jumper>().Dying();
        jumpAnim.Play("LevelComplete", 0, 0);
        jumper.GetComponent<Jumper>().Dying();
    }
}
