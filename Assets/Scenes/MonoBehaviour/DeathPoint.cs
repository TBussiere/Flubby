using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    public CheckPointsHandler cph;
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
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Destroy(other.gameObject);
            cph.ResetToCP();
            Debug.Log("dead");
        }
    }
}
