using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKIP : MonoBehaviour
{
    public GameObject blob;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S))
        {
            bring();
        }
    }

    void bring()
    {
        blob.transform.position = this.transform.position;
        foreach (Transform t in blob.transform)
        {
            t.position = this.transform.position;
        }
    }
}
