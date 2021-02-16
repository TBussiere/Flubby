using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    private CheckPointsHandler cph;
    public bool crunchDeath = false;
    public bool playerDetected = false;
    public bool groundDetected = false;
    public bool mouving = false;
    private Rigidbody2D rb;

    public float playerRelativePosition;

    private bool once=true;
    private int playerParticule = 0;

    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Assertions.Assert.IsNotNull(cph, "Script DeathPoint needs ref to CheckPointsHandler. obj : " + this.name);
        var test = FindObjectsOfType<CheckPointsHandler>();
        if (test.Length > 1 || test.Length < 0)
        {
            Debug.LogError("CheckPointsHandler not found in the scene @" + this.name);
        }
        cph = test[0];

        if (crunchDeath)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (crunchDeath)
        {
            if (!once)
                return;

            mouving = rb.velocity.magnitude > 0.5f;

            if (playerDetected && mouving && groundDetected)
            {
                if ((Mathf.Sign(playerRelativePosition) == Mathf.Sign(rb.velocity.x)))
                {
                    cph.playReset();
                    once = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!once)
            return;

        if (crunchDeath)
        {
            if (other.tag == "Player")
            {
                playerDetected = true;

                Vector3 distance = other.transform.position - transform.position;
                playerRelativePosition = Vector2.Dot(distance.normalized, transform.right.normalized);

                playerParticule++;
            }
            else if (other.tag == "Ground")
            {
                groundDetected = true;
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                cph.playReset();
                once = false;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && crunchDeath)
        {
            playerParticule--;
            if (playerParticule == 0)
            {
                playerDetected = false;
            }   
        }
        else if (other.tag == "Ground")
        {
            groundDetected = false;
        }
    }
}
