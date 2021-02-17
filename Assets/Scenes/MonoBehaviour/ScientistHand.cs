using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistHand : MonoBehaviour
{
    public BoundingBox BlobBBox;
    public Vector2 blob_position;

    public float sprite_H_offset;
    public float sprite_V_offset;
    public float down_speed;
    public float up_speed;
    public float lateral_speed;
    public float up_max_dist;
    public bool direction; // true is down, false is up

    public bool triggered;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ComputeBlobPosition();
        Follow();
        if (triggered)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            triggered = true;
        }
    }

    void ComputeBlobPosition()
    {
        blob_position = (BlobBBox.bottom_left_corner + BlobBBox.top_right_corner) / 2f;
    }

    void GetDown()
    {
        transform.position = transform.position - new Vector3(0, down_speed, 0);
    }

    void GetUp()
    {
        transform.position = transform.position + new Vector3(0, up_speed, 0);
    }

    void Follow()
    {
        transform.position = transform.position + (new Vector3(blob_position.x - transform.position.x + sprite_H_offset, 0, 0) * lateral_speed);
    }

    void Move()
    {
        if (direction)
        {
            GetDown();
            if (transform.position.y < blob_position.y + sprite_V_offset) direction = false;
        }
        else
        {
            GetUp();
            if (transform.position.y > blob_position.y + up_max_dist + sprite_V_offset) direction = true;
        }
    }


}
