using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    public GameObject blob = null;
    BlobView blob_view;
    BoxCollider2D box_collider;


    /****************** Bounding Box parameters ******************/
    Bounds bounds_box;
    // ceux pour BlobRenderer
    public Vector2 top_right_corner;
    public Vector2 bottom_left_corner;

    // utilisés pour calculer la bounding box
    public Vector2 top_left_corner;
    public Vector2 bottom_right_corner;

    public float offset = 1;

    /****************** Mouse Down and Drag parameters ******************/
    public float max_size = 2;
    public float coef_max = 0.5f;
    public float coef_min = 0.2f;
    public float delta_t = 1;
    private float delta_clicked = 0;
    Dictionary<int, Rigidbody2D> ParticuleToRigidbody2D = new Dictionary<int, Rigidbody2D>();
    bool dico_filled = false;
    GameObject last_particule;
    bool clicked = false;


    // Start is called before the first frame update
    void Start()
    {
        blob_view = blob.GetComponentInChildren<BlobView>();
        box_collider = GetComponentInParent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dico_filled)
        {
            foreach (GameObject p in blob_view.particules)
            {
                ParticuleToRigidbody2D.Add(p.GetInstanceID(), p.GetComponent<Rigidbody2D>());
            }
            dico_filled = true;
        }

        ComputeBoundingBox();

        top_left_corner = new Vector2(bottom_left_corner.x, top_right_corner.y);
        bottom_right_corner = new Vector2(top_right_corner.x, bottom_left_corner.y);
        bounds_box.SetMinMax(top_left_corner, bottom_right_corner);
        bounds_box.size = new Vector2(top_right_corner.x - bottom_left_corner.x, top_right_corner.y - bottom_left_corner.y);

        transform.position = bounds_box.center;

        if (Input.GetMouseButton(0))
        {
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clicked && last_particule != null)
            {
                // Drag
                MouseDrag(MousePos);
            }
            else if (bounds_box.Contains(MousePos))
            {
                // Recupère la plus proche
                clicked = true;
                MouseIsDown(MousePos);
            }
        }
        else
            clicked = false;

    }

    GameObject GetClosestParticule(Vector2 MousePos)
    {
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
        return particule;
    }

    void MouseIsDown(Vector2 MousePos)
    {
        delta_clicked = Time.time;
        last_particule = GetClosestParticule(MousePos);
    }

    void MouseDrag(Vector3 MousePos)
    {
        float time_elapsed = Mathf.Min(Time.time - delta_clicked, delta_t);
        float coef = coef_max - (coef_max - coef_min) * (time_elapsed / delta_t);
        Vector3 direction = MousePos - last_particule.transform.position;
        float magnitude = Mathf.Min(direction.magnitude, max_size);
        Vector3 force = coef * magnitude * direction.normalized;
        Debug.Log("Force appliquée : " + force);
        ParticuleToRigidbody2D[last_particule.GetInstanceID()].AddForce(force, ForceMode2D.Impulse);
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
