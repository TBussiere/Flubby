using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float max_size = 2;
    public float coef_max = 0.5f;
    public float coef_min = 0.2f;
    public float delta_t = 1;
    public float coef;
    private Rigidbody2D rb2D;
    private float when_clicked = 0; 
    
    public Color lineColor = Color.red;

    Material lineMaterial;

    void Awake()
    {
        // must be called before trying to draw lines..
        CreateLineMaterial();
    }

    void CreateLineMaterial()
    {
        // Unity has a built-in shader that is useful for drawing simple colored things
        var shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        lineMaterial.SetInt("_ZWrite", 0);
    }

    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        coef = coef_max;
    }

    void OnMouseDown()
    {
        // Reinit parameters
        when_clicked = Time.time;
        max_size = 5;
        coef = coef_max;

        //Debug.Log(this.gameObject.name);
    }
    void drag()
    {
        float time_elapsed = Mathf.Min(Time.time - when_clicked, delta_t);
        coef = coef_max - (coef_max - coef_min) * (time_elapsed / delta_t);
        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouse_pos - this.transform.position;

        float magnitude = Mathf.Min(direction.magnitude, max_size);

        rb2D.AddForce(coef * magnitude * direction.normalized, ForceMode2D.Impulse);

        // Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Vector3 drag_vector = mouse_pos - this.transform.position;

        // float drag_magnitude = drag_vector.magnitude;
        // Vector3 drag_direction = drag_vector.normalized;
        // // float magnitude = Mathf.Min(direction.magnitude, max_drag_distance);
        // float magnitude = Mathf.Min(drag_magnitude, fmax);

        // rb2D.AddForce(magnitude * drag_direction, ForceMode2D.Impulse);

    }

    
    void OnMouseDrag()
    {
        drag();
        /*foreach(SpringJoint2D spring in GetComponents<SpringJoint2D>())
        {
            spring.connectedBody.GetComponentInParent<Controls>().drag();
        }*/
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject blob = this.transform.parent.gameObject;
            BlobView bv = blob.GetComponent<BlobView>();
            bv.try_link(this.gameObject, collision.gameObject);
        }
    }

    #if false
    void OnRenderObject()
    {
        lineMaterial.SetPass(0);

        GL.PushMatrix();


        Vector3 part_pos = this.transform.position;
        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GL.Begin(GL.LINES);
        GL.Color(new Color(1f, 1f, 1f, 1f));
        GL.Vertex3(mouse_pos.x, mouse_pos.y, 0);
        GL.Vertex3(part_pos.x, part_pos.y, 0);
        GL.End();


        GL.PopMatrix();
    }
    #endif
}
