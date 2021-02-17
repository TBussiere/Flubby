using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marche : MonoBehaviour
{

    bool Passed;
    MarchesHandler Handler;
    // Start is called before the first frame update
    void Start()
    {
        Handler = GetComponentInParent<MarchesHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Passed == false)
        {
            if (Handler.safe)
            {
                Passed = true;
                Handler.changeState();
            }
            else
            {
                Handler.RingAlarm();
            }
        }
    }
}
