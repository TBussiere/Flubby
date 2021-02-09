using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobRenderer : MonoBehaviour
{
    // Coordonn�s de la grille autour du blob
    public Vector2 bottom_left_corner;
    public Vector2 top_right_corner;
    public float square_size = 0.1f;
    public float offset = 1;
    public float radius_blob = 1.0f;
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
#if false
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
#if true
        if (square_size > 0)
        {
            for (float x_travel = bottom_left_corner.x; x_travel < top_right_corner.x; x_travel += square_size)
            {
                for (float y_travel = bottom_left_corner.y; y_travel < top_right_corner.y; y_travel += square_size)
                {
                    // Square points reference :
                    //
                    // D k C
                    // l . j
                    // A i B


                    // Sommets
                    var A = new Vector2(x_travel, y_travel);
                    var B = new Vector2(x_travel + square_size, y_travel);
                    var C = new Vector2(x_travel + square_size, y_travel + square_size);
                    var D = new Vector2(x_travel, y_travel + square_size);

                    // Milieu des segments
                    var I = new Vector2(x_travel + square_size * 0.5f, y_travel);
                    var J = new Vector2(x_travel + square_size, y_travel + square_size * 0.5f);
                    var K = new Vector2(x_travel + square_size * 0.5f, y_travel + square_size);
                    var L = new Vector2(x_travel, y_travel + square_size * 0.5f);

                    int sa = isBlobCollider(A);
                    int sb = isBlobCollider(B);
                    int sc = isBlobCollider(C);
                    int sd = isBlobCollider(D);

                    int state = GetState(sa, sb, sc, sd);

                    switch (state)
                    {
                        case 1:
                            DrawTriangle(K, D, L);
                            break;
                        case 2:
                            DrawTriangle(J, C, K);
                            break;
                        case 3:
                            DrawTriangle(J, D, L);
                            DrawTriangle(D, J, C);
                            break;
                        case 4:
                            DrawTriangle(I, B, J);
                            break;
                        case 5:
                            DrawTriangle(B, L, I);
                            DrawTriangle(B, D, L);
                            DrawTriangle(B, K, D);
                            DrawTriangle(B, J, K);
                            break;
                        case 6:
                            DrawTriangle(B, C, K);
                            DrawTriangle(K, I, B);
                            break;
                        case 7:
                            DrawTriangle(C, D, L);
                            DrawTriangle(C, L, I);
                            DrawTriangle(C, I, B);
                            break;
                        case 8:
                            DrawTriangle(A, I, L);
                            break;
                        case 9:
                            DrawTriangle(A, K, D);
                            DrawTriangle(A, I, K);
                            break;
                        case 10:
                            DrawTriangle(A, I, J);
                            DrawTriangle(A, J, C);
                            DrawTriangle(A, C, K);
                            DrawTriangle(A, K, L);
                            break;
                        case 11:
                            DrawTriangle(D, A, I);
                            DrawTriangle(D, I, J);
                            DrawTriangle(D, J, C);
                            break;
                        case 12:
                            DrawTriangle(L, A, J);
                            DrawTriangle(A, B, J);
                            break;
                        case 13:
                            DrawTriangle(A, B, J);
                            DrawTriangle(A, J, K);
                            DrawTriangle(A, K, D);
                            break;
                        case 14:
                            DrawTriangle(B, C, K);
                            DrawTriangle(B, K, L);
                            DrawTriangle(B, L, A);
                            break;
                        case 15:
                            DrawTriangle(A, B, C);
                            DrawTriangle(A, C, D);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
#endif

        GL.PopMatrix();
    }

    void DrawTriangle(Vector2 a, Vector2 b, Vector2 c)
    {
        GL.Begin(GL.TRIANGLES);
        GL.Color(lineColor);

        GL.Vertex3(a.x, a.y, 0);
        GL.Vertex3(b.x, b.y, 0);
        GL.Vertex3(c.x, c.y, 0);

        GL.End();
    }

    // convert "binary" to int (0 0 0 0 = 0)
    int GetState(int a, int b, int c, int d)
    {
        return a * 8 + b * 4 + c * 2 + d * 1;
    }

    int isBlobCollider(Vector2 p)
    {
        Ray ray = new Ray(new Vector3(p.x, p.y, -10), transform.forward);

        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            return 1;
        }

        return 0;
    }

    float falloff(float x, float R)
    {

        float u = Mathf.Clamp(x / R, 0.0f, 1.0f);
        float v = (1.0f - u * u);
        return v * v * v;
    }

    float blend(float d1, float d2)
    {
        return d1 + d2;
    }

    float sphere(Vector2 centre, float radius, Vector2 p)
    {
        return falloff((centre - p).magnitude, radius);
    }

    float SI(Vector2 p)
    {
        float v = 0.0f;

        for (int i = 0; i < blob.transform.childCount; i++)
        {
            Vector2 pos_particule = blob.transform.GetChild(i).position;
            float s = sphere(pos_particule, radius_blob, p);
            v = blend(v, s);
        }

        return v;
    }

}
