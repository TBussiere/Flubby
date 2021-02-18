using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushScript : MonoBehaviour
{
    public MarchesHandler marchesHandler;
    bool triggered;

    public float timeUntilHand = 10;

    

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(marchesHandler, "Script RushScript needs ref to MarchesHandler. obj : " + this.name);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered == false)
        {
            triggered = true;
            marchesHandler.timeUntilHand = this.timeUntilHand;
            marchesHandler.RingAlarm();
        }
    }
}
