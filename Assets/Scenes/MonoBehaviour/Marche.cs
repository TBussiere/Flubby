using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marche : MonoBehaviour
{

    public bool Passed;
    MarchesHandler Handler;
    // Start is called before the first frame update
    void Start()
    {
        Handler = GetComponentInParent<MarchesHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Handler.enableHand();
        if (Passed == false)
        {
            Passed = true;
            Handler.playSplouch();

            if (Handler.safe)
            {
                Handler.changeState();
            }
            else
            {
                Handler.RingAlarm();
            }
        }
    }
}
