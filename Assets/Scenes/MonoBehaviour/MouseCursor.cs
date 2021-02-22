using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    // Start is called before the first frame update*
    SpriteRenderer spriteRenderer;
    public Vector2 offset;
    public Sprite still;
    public Sprite holding;

    void Start()
    {
        Cursor.visible = false;
        // Cursor.SetCursor(null, new Vector2(), CursorMode.ForceSoftware);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = MousePos + offset;
        if (Input.GetMouseButton(0))
            spriteRenderer.sprite = holding;
        else
            spriteRenderer.sprite = still;


    }

}
