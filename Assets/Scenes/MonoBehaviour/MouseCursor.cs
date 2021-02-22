using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    // Start is called before the first frame update*
    SpriteRenderer spriteRenderer;
    public Texture2D open;
    public Texture2D grab;
    public Vector2 offset;

    void Start()
    {
        //Cursor.visible = false;
        // Cursor.SetCursor(null, new Vector2(), CursorMode.ForceSoftware);
        //spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.SetCursor(open, offset, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = MousePos + offset;*/
        if (Input.GetMouseButton(0))
            Cursor.SetCursor(grab, offset, CursorMode.ForceSoftware);
        else
            Cursor.SetCursor(open, offset, CursorMode.ForceSoftware);
        /*spriteRenderer.sprite = holding;
        else
            spriteRenderer.sprite = still;*/
    }

}
