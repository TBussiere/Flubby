using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInsideCollider : MonoBehaviour
{
    public bool PlayerInside;
    public float delay;

    private float timer;
    private bool has_exit;

    void Update()
    {
        float t = Time.time;
        if (has_exit && t - timer > delay)
        {
            PlayerInside = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInside = true;
            has_exit = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer = Time.time;
            has_exit = true;
        }
    }
}
