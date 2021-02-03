using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public bool particle_selected;
    public float max_size;
    public Vector3 mouse_pos;
    private Rigidbody2D rb2D;

    void Start()
    {
        particle_selected = false;
        mouse_pos = Vector3.zero;
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.MouseUp)
        {
            particle_selected = false;
        }
    }

    void OnMouseDrag()
    {
        mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        particle_selected = true;

        Vector3 direction = mouse_pos - this.transform.position;
        rb2D.AddForce(0.01f * direction, ForceMode2D.Impulse);
    }

    void OnRenderObject()
    {
        Vector3 part_pos = this.transform.position;

        GL.Begin(GL.LINES);
        GL.Color(new Color(1f, 0f, 0f, 1f));
        GL.Vertex3(mouse_pos.x, mouse_pos.y, mouse_pos.z);
        GL.Vertex3(part_pos.x, part_pos.y, part_pos.z);
        GL.End();
    }
}
