using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    public Vector2 bottom_left_corner;
    public Vector2 top_right_corner;
    public float offset = 1;
    public GameObject blob = null;

    BlobView blob_view;
    BoxCollider2D box_collider;

    public GameObject camera;
    BlobRenderer blob_renderer;

    public float distance_max_autorisee = 2f;
    Vector2 last_pos_click;
    public GameObject last_particule;

    Dictionary<int, Controls> ParticuleToControls = new Dictionary<int, Controls>();

    public bool clicked;

    // Start is called before the first frame update
    void Start()
    {
        // bottom_left_corner = new Vector2();
        // top_right_corner = new Vector2();
        blob_view = blob.GetComponentInChildren<BlobView>();
        box_collider = GetComponentInParent<BoxCollider2D>();
        blob_renderer = camera.GetComponent<BlobRenderer>();
        // Debug.Log(blob_renderer);
        
        foreach(GameObject p in blob_view.particules)
        {
            ParticuleToControls.Add(p.GetInstanceID(), p.GetComponent<Controls>());
        }

        clicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        ComputeBoundingBox();


        Vector2 center = (bottom_left_corner + top_right_corner) / 2;
        transform.position = center;
        // Debug.Log(box_collider);
        box_collider.size = new Vector2(top_right_corner.x - bottom_left_corner.x, top_right_corner.y - bottom_left_corner.y);

        // box_collider.bounds.center = center;
        // box_collider.bounds.SetMinMax
        Vector2 top_left_corner = new Vector2(bottom_left_corner.x, top_right_corner.y);
        Vector2 bottom_right_corner = new Vector2(top_right_corner.x, bottom_left_corner.y);

        box_collider.bounds.SetMinMax(top_left_corner, bottom_right_corner);
        // Debug.Log("bottom left : " + bottom_left_corner);
        // Debug.Log("top right : " + top_right_corner);

        if (Input.GetMouseButton(0))
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (si on est dans la bbox) {
                if (clicked == false)
                {
                    // Recupère la plus proche
                    clicked = true;
                }
                else
                {
                    // Drag
                }
            }
        }
        else
        {
            clicked = false;
        }
    }

    GameObject GetClosestParticule(Vector2 MousePos)
    {
        //if (blob_renderer.isBlobSI(MousePos) == 1)
        //{
            GameObject particule = blob_view.particules[0];
            Vector2 pos2D = new Vector2(particule.transform.position.x, particule.transform.position.y);
            float min_distance = (pos2D - MousePos).magnitude;

            for (int i = 1; i < blob_view.particules.Count; ++i)
            {
                GameObject p = blob_view.particules[i];
                pos2D = new Vector2(p.transform.position.x, p.transform.position.y);
                float d = (pos2D - MousePos).magnitude;
                Debug.Log(d);
                if (d < min_distance)
                {
                    min_distance = d;
                    particule = p;
                }
            }

            Debug.Log(particule);
            return particule;

        //}
        //return null;
    }

    void MouseIsDown()
    {
        Debug.Log("Drag box collider");
        // https://forum.unity.com/threads/onmousedrag.522567/
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // GameObject p = GetClosestParticule(MousePos);
        // if (p != null)
        //     p.GetComponent<Controls>().OnMouseDown();

        /*if (last_particule == null || (last_pos_click - MousePos).magnitude > distance_max_autorisee)
        {
            last_particule = GetClosestParticule(MousePos);
            last_pos_click = MousePos;
        }

        if (last_particule != null)
            last_particule.GetComponent<Controls>().OnMouseDown();*/


        last_particule = GetClosestParticule(MousePos);
    }

    void MouseDrag()
    {
        /*Debug.Log("drag bb");
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // GameObject p = GetClosestParticule(MousePos);
        // if (p != null)
        //     p.GetComponent<Controls>().drag();
        if (last_particule == null || (last_pos_click - MousePos).magnitude > distance_max_autorisee)
        {
            last_particule = GetClosestParticule(MousePos);
            last_pos_click = MousePos;
        }

        if (last_particule != null)
        {
            // Debug.Log("last = " + last_particule.transform.position);
            last_particule.GetComponent<Controls>().drag();
        }*/
    }

    void ComputeBoundingBox()
    {
        bottom_left_corner = blob.transform.GetChild(0).position;
        top_right_corner = blob.transform.GetChild(0).position;

        for (int i = 0; i < blob.transform.childCount; i++)
        {
            Transform child = blob.transform.GetChild(i);

            if (child.position.x <= bottom_left_corner.x)
            {
                bottom_left_corner.x = child.position.x;
            }
            else if (child.position.x >= top_right_corner.x)
            {
                top_right_corner.x = child.position.x;
            }

            if (child.position.y <= bottom_left_corner.y)
            {
                bottom_left_corner.y = child.position.y;
            }
            else if (child.position.y >= top_right_corner.y)
            {
                top_right_corner.y = child.position.y;
            }
        }

        bottom_left_corner -= new Vector2(offset, offset);
        top_right_corner += new Vector2(offset, offset);
    }
}
