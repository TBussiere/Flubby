using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    public CheckPointsHandler cph;
    public bool crunchDeath = false;
    public bool playerDetected = false;
    public bool mouving = false;
    private Rigidbody2D rb;

    private bool once=true;

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

            if (playerDetected && mouving)
            {
                Debug.Log("Mouving et player");
                RaycastHit2D[] hits;
                Vector2 sensChute = new Vector2(transform.position.x, transform.position.y) + rb.velocity;
                //Ray2D r = new Ray2D(transform.position, sensChute);

                hits = Physics2D.RaycastAll(transform.position, sensChute);

                foreach (var hit in hits)
                {
                    if (hit.transform.tag == "Player")
                    {
                        cph.playReset();
                        once = false;
                    }
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
        if (other.tag == "Player")
        {
            playerDetected = false;
        }
    }
}
