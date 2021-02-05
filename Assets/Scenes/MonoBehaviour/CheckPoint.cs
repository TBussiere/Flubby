using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool Passed;
    CheckPointsHandler Handler;
    // Start is called before the first frame update
    void Start()
    {
        Handler = GetComponentInParent<CheckPointsHandler>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Passed == false)
        {
            if (other.tag == "Player")
            {
                Debug.Log(this.name + " has been passed");
                Passed = true;
                Handler.CPpass(this.gameObject);
            }
        }
    }
}
