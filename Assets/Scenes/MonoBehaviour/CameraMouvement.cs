using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    public GameObject blob;
    public BoundingBox bbox;
    public bool zoom_in;
    public bool level1;

    public float size_zoom;
    public float coef;
    private float timer;

    private Vector3 initial_cam_position;
    private float initial_cam_size;
    private Vector3 initial_offset;

    // Lvl 1 parameters
    public bool zoom1to2;
    public float zoomSpeed1to2;
    public float size_1 = 13.3f;
    public Vector3 offsetLvl1 = new Vector3(5, 5, 0);

    // Lvl 2 parameters
    private bool zoom2to1;
    public float zoomSpeed2to1;
    public float size_2 = 42.25f;
    public Vector3 pos2 = new Vector3(177, -100, -80);


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
    }

    public void SwitchView()
    {
        if (zoom1to2)
            return;

        initial_cam_position = this.transform.position;
        initial_cam_size = this.GetComponent<Camera>().orthographicSize;
        timer = Time.time;

        if (level1)
        {
            level1 = false;
            zoom2to1 = false;
            zoom1to2 = true;
        }
        else
        {
            /*level1 = true;
            zoom1to2 = false;
            zoom2to1 = true;*/
        }
    }

    void Zoom_1_to_2()
    {
        float alpha = (Time.time - timer) / zoomSpeed1to2;
        alpha = Mathf.Pow(alpha, 3);
        Debug.Log(alpha);

        Vector3 dist_pos = pos2 - initial_cam_position;
        Debug.Log(dist_pos);
        transform.position = initial_cam_position + dist_pos * alpha;

        float dist_size = size_2 - initial_cam_size;
        this.GetComponent<Camera>().orthographicSize = initial_cam_size + dist_size * alpha;

        if (alpha >= 1)
        {
            zoom1to2 = false;
        }
    }

    void Zoom_2_to_1()
    {
        float alpha = (Time.time - timer) / zoomSpeed2to1;

        Vector3 blob_center = (bbox.top_right_corner + bbox.bottom_left_corner) / 2f;
        Vector3 dist_pos = blob_center - initial_cam_position;
        transform.position = offsetLvl1 + initial_cam_position + dist_pos * alpha;

        float dist_size = size_1 - initial_cam_size;
        this.GetComponent<Camera>().orthographicSize = initial_cam_size + dist_size * alpha;


        if (alpha >= 1)
        {
            zoom1to2 = false;
        }
    }

    public void ResetParameters()
    {
        if(!level1)
        {
            Camera cam = this.GetComponent<Camera>();
            cam.orthographicSize = size_2;
            this.transform.position = pos2;
            zoom_in = false;
        }
        
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

        this.gameObject.transform.position = mean_children_position + offsetLvl1;
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -80);
    }

    // Update is called once per frame
    void Update()
    {
        if (zoom_in)
        {
            zoom();
        }
        else if (zoom1to2)
        {
            Zoom_1_to_2();
        }
        else if (zoom2to1)
        {
            Zoom_2_to_1();
        }
        else if (level1)
        {
            Follow();
        }
    }
}

