using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveling : MonoBehaviour
{
    public CameraMouvement cam;
    public float speed;
    bool trav;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CameraMouvement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            pos = transform.position;
            cam.enabled = false;
            trav = true;
            transform.position = new Vector3(151, transform.position.y, transform.position.z);
        }
        if (trav)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x < pos.x)
            {
                trav = false;
                cam.enabled = true;
            }
        }
    }
}
