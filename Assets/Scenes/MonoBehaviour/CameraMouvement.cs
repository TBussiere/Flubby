using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    public GameObject blob;
    public BoundingBox bbox;
    public bool zoom_in;

    public float size_zoom;
    public float x_speed;
    public float y_speed;
    public float coef;

    private float size_ = 42.25f;
    private float x_ = 177;
    private float y_ = -100;

    public float A;
    public float B;

    void zoom()
    {
        Vector3 blob_center = (bbox.top_right_corner + bbox.bottom_left_corner) / 2f;
        blob_center = new Vector3(blob_center.x, blob_center.y, -80);

        Vector3 dist = blob_center - transform.position;

        if (dist.magnitude > 0.3f)
        {
            transform.position = transform.position + dist * coef;
        }

        Camera cam = this.GetComponent<Camera>();
        float dist_ = size_zoom - cam.orthographicSize;

        cam.orthographicSize = cam.orthographicSize + dist_ * coef;

        if (dist.magnitude < 0.3f && dist_ < 0.3f)
        {
            //zoom_in = false;
        }

        A = dist.magnitude;
        B = dist_;
    }

    public void ResetParameters()
    {
        Camera cam = this.GetComponent<Camera>();
        cam.orthographicSize = size_;
        this.transform.position = new Vector3(x_, y_, -80);
        this.enabled = false;
    }

    void Follow()
    {
        Vector3 mean_children_position = Vector3.zero;
        foreach (Transform child in blob.transform)
        {
            GameObject obj = child.gameObject;
            mean_children_position += obj.transform.position;
        }
        mean_children_position /= blob.transform.childCount;

        this.gameObject.transform.position = mean_children_position + new Vector3(5, 5, -80);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -80);
    }

    // Update is called once per frame
    void Update()
    {
        if (zoom_in)
        {
            zoom();
        }
        else
        {
            Follow();
        }
    }
}

