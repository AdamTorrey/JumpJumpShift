using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [Header("Next Level")]
    [SerializeField] string nextScene;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedWith = collision.gameObject; // If the jumper collides with this
        if (collidedWith.CompareTag("Jumper"))
        {
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.g = 0;
            c.b = 0;
            c.r = 0;
            mat.color = c;
            collision.collider.GetComponent<Death>().LevelComplete();
            Invoke("LoadNextLevel", 3);
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}
