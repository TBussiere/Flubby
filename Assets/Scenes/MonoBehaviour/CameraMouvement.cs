using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    public GameObject blob;

    // Start is called before the first frame update
    void Start()
    {
        //blob = GameObject.Find("Blob");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mean_children_position = Vector3.zero;
        foreach (Transform child in blob.transform)
        {
            GameObject obj = child.gameObject;
            mean_children_position += obj.transform.position;
        }
        mean_children_position /= blob.transform.childCount;

        this.gameObject.transform.position = mean_children_position + new Vector3(5,10,0);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -10) ;
    }
}
