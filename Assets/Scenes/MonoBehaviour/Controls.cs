using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float max_size = 2;
    public float coef_max = 0.09f;
    public float coef_min = 0.07f;
    public float delta_t = 1;
    public float coef;
    private Rigidbody2D rb2D;
    private float when_clicked = 0;

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
    }

    void OnMouseDrag()
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
        // Debug.Log(this.gameObject.name);
    }

    #if false
    void OnRenderObject()
    {
        Vector3 part_pos = this.transform.position;
        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouse_pos - this.transform.position;

        float magnitude = Mathf.Min(direction.magnitude, max_size);
        mouse_pos = this.transform.position + direction.normalized * magnitude;

        GL.Begin(GL.LINES);
        GL.Color(new Color(1f, 1f, 1f, 1f));
        GL.Vertex3(mouse_pos.x, mouse_pos.y, mouse_pos.z);
        GL.Vertex3(part_pos.x, part_pos.y, part_pos.z);
        GL.End();
    }
    #endif
}
