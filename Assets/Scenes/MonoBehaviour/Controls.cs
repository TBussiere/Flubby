using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float max_size = 5;
    public float coef = 0.09f;
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        
    }

    void OnMouseDown()
    {
        // Reinit parameters
        coef = 0.09f;
    }

    void OnMouseDrag()
    {
        coef = Mathf.Max(0.05f, coef - 0.001f);
        Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouse_pos - this.transform.position;

        float magnitude = Mathf.Min(direction.magnitude, max_size);

        rb2D.AddForce(coef * magnitude * direction.normalized, ForceMode2D.Impulse);
        //Debug.Log(this.gameObject.name);
    }

    #if true
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
