using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobRenderer : MonoBehaviour
{
    // Coordonnés de la grille autour du blob
    public Vector2 bottom_left_corner;
    public Vector2 top_right_corner;
    public float square_size = 0.3f;
    public float offset = 1;
    public GameObject blob;

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

    // Update is called once per frame
    void Update()
    {
        ComputeBoundingBox();
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

    void OnPostRender()
    {
        if (!lineMaterial)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

        GL.PushMatrix();
        lineMaterial.SetPass(0);

        // Show grid
#if true
        GL.Begin(GL.LINES);
        GL.Color(lineColor);

        for (float x_travel = bottom_left_corner.x; x_travel < top_right_corner.x; x_travel += square_size)
        {
            GL.Vertex3(x_travel, bottom_left_corner.y, 0);
            GL.Vertex3(x_travel, top_right_corner.y, 0);
        }

        for (float y_travel = bottom_left_corner.y; y_travel < top_right_corner.y; y_travel += square_size)
        {
            GL.Vertex3(bottom_left_corner.x, y_travel, 0);
            GL.Vertex3(top_right_corner.x, y_travel, 0);
        }
        GL.End();
#endif

        // Marching triangle based on colliders - WIP
#if false
        global.Begin(global.TRIANGLES);
        GL.Color(lineColor);

        for (float x_travel = bottom_left_corner.x; x_travel < top_right_corner.x; x_travel += square_size)
        {
            for (float y_travel = bottom_left_corner.y; y_travel < top_right_corner.y; y_travel += square_size)
            {
                var a = new Vector2(x_travel + square_size * 0.5f,  y_travel);
                var b = new Vector2(x_travel + square_size,         y_travel + square_size * 0.5f);
                var c = new Vector2(x_travel + square_size * 0.5f,  y_travel + square_size);
                var d = new Vector2(x_travel,                       y_travel + square_size * 0.5f);

                int sa = 0;//Mathf.CeilToInt(field[i, j]);
                int sb = 0;//Mathf.CeilToInt(field[i + 1, j]);
                int sc = 0;//Mathf.CeilToInt(field[i + 1, j + 1]);
                int sd = 1;//Mathf.CeilToInt(field[i, j + 1]);

                int state = GetState(sa, sb, sc, sd);

                switch (state)
                {
                    case 1:
                        Line(c, d);
                        break;
                    case 2:
                        Line(b, c);
                        break;
                    case 3:
                        Line(b, d);
                        break;
                    case 4:
                        Line(a, b);
                        break;
                    case 5:
                        Line(a, d);
                        Line(b, c);
                        break;
                    case 6:
                        Line(a, c);
                        break;
                    case 7:
                        Line(a, d);
                        break;
                    case 8:
                        Line(a, d);
                        break;
                    case 9:
                        Line(a, c);
                        break;
                    case 10:
                        Line(a, b);
                        Line(c, d);
                        break;
                    case 11:
                        Line(a, b);
                        break;
                    case 12:
                        Line(b, d);
                        break;
                    case 13:
                        Line(b, c);
                        break;
                    case 14:
                        Line(c, d);
                        break;
                    default:
                        break;
                }
            }
        }

        GL.End();
#endif

        GL.PopMatrix();
    }

    void Line(Vector2 a, Vector2 b)
    {
        Debug.DrawLine(a, b, Color.white);
    }

    // convert "binary" to int (0 0 0 0 = 0)
    int GetState(int a, int b, int c, int d)
    {
        return a * 8 + b * 4 + c * 2 + d * 1;
    }
}
