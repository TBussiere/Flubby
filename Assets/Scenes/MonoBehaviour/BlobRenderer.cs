using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobRenderer : MonoBehaviour
{
    // Coordonn�s de la grille autour du blob
    
    public float square_size = 0.1f;
    
    public float radius_blob = 1.0f;
    public GameObject blob;
    public BoundingBox blob_bb;

    public int width;
    public int height;

    public Color insideColor = Color.green;
    public Color lineColor = Color.black;

    Material lineMaterial;

    void Start()
    {
        // blob_bb = blob.GetComponent<BoundingBox>();
    }
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
        ComputeGridSize();

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            square_size = Mathf.Min(0.6f, square_size + 0.05f);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            square_size = Mathf.Max(0.1f, square_size - 0.05f);
        }
    }

    void OnRenderObject()
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

        for (float x_travel = blob_bb.bottom_left_corner.x; x_travel < blob_bb.top_right_corner.x; x_travel += square_size)
        {
            GL.Vertex3(x_travel, blob_bb.bottom_left_corner.y, 0);
            GL.Vertex3(x_travel, blob_bb.top_right_corner.y, 0);
        }

        for (float y_travel = blob_bb.bottom_left_corner.y; y_travel < blob_bb.top_right_corner.y; y_travel += square_size)
        {
            GL.Vertex3(blob_bb.bottom_left_corner.x, y_travel, 0);
            GL.Vertex3(blob_bb.top_right_corner.x, y_travel, 0);
        }
        GL.End();
#endif

        // Marching triangle based on colliders - WIP
#if true
        if (square_size > 0)
        {
            float[,] gridValues = ComputeGrid();

            for (int x = 0; x < width - 1; x++)
            {
                for (int y = 0; y < height - 1; y++)
                {
                    Vector2 A = GridToWorld(x,      y);
                    Vector2 B = GridToWorld(x + 1,  y);
                    Vector2 C = GridToWorld(x + 1,  y + 1);
                    Vector2 D = GridToWorld(x,      y + 1);
                    DrawSquare(A, B, C, D, gridValues[x, y], gridValues[x + 1, y], gridValues[x + 1, y + 1], gridValues[x, y + 1]);
                }
            }
        }
#endif

        GL.PopMatrix();
    }

    void ComputeGridSize()
    {
        if (square_size > 0)
        {
            width = (int)(Mathf.Abs(blob_bb.bottom_left_corner.x - blob_bb.top_right_corner.x) / square_size);
            height = (int)(Mathf.Abs(blob_bb.bottom_left_corner.y - blob_bb.top_right_corner.y) / square_size);
        }
    }

    float[,] ComputeGrid()
    {
        float[,] gridVlaues = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridVlaues[x, y] = SI(new Vector2(blob_bb.bottom_left_corner.x + x * square_size, blob_bb.bottom_left_corner.y + y * square_size));
            }
        }

        return gridVlaues;
    }

    void DrawSquare(Vector2 A, Vector2 B, Vector2 C, Vector2 D, float va, float vb, float vc, float vd)
    {
        // Square points reference :
        //
        // D K C
        // L . J
        // A I B

        // Milieu des segments
        var I = SurfacePositionEstimation(A, va, B, vb);
        var J = SurfacePositionEstimation(B, vb, C, vc);
        var K = SurfacePositionEstimation(C, vc, D, vd);
        var L = SurfacePositionEstimation(D, vd, A, va);

        int state = GetState(va, vb, vc, vd);
        //Debug.Log("======== State = " + va + " " + vb + " " + vc + " " + vd);
        //Debug.Log("======== State = " + state);

        // Inside color
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

        // Edges
        switch (state)
        {
            case 1:
                DrawThickLine(L, K);
                break;
            case 2:
                DrawThickLine(K, J);
                break;
            case 3:
                DrawThickLine(L, J);
                break;
            case 4:
                DrawThickLine(I, J);
                break;
            case 5:
                DrawThickLine(L, I);
                DrawThickLine(J, K);
                break;
            case 6:
                DrawThickLine(I, K);
                break;
            case 7:
                DrawThickLine(L, I);
                break;
            case 8:
                DrawThickLine(L, I);
                break;
            case 9:
                DrawThickLine(K, I);
                break;
            case 10:
                DrawThickLine(L, K);
                DrawThickLine(J, I);
                break;
            case 11:
                DrawThickLine(J, I);
                break;
            case 12:
                DrawThickLine(L, J);
                break;
            case 13:
                DrawThickLine(K, J);
                break;
            case 14:
                DrawThickLine(L, K);
                break;
            default:
                break;
        }
    }


    void DrawTriangle(Vector2 a, Vector2 b, Vector2 c)
    {
        GL.Begin(GL.TRIANGLES);
        GL.Color(insideColor);

        GL.Vertex3(a.x, a.y, 0);
        GL.Vertex3(b.x, b.y, 0);
        GL.Vertex3(c.x, c.y, 0);

        GL.End();
    }

    void DrawLine(Vector2 a, Vector2 b)
    {
        GL.Begin(GL.LINES);
        GL.Color(lineColor);

        GL.Vertex3(a.x, a.y, 0);
        GL.Vertex3(b.x, b.y, 0);

        GL.End();
    }

    void DrawThickLine(Vector2 a, Vector2 b)
    {
        GL.Begin(GL.LINES);
        GL.Color(lineColor);

        for (float i = -0.05f; i < 0.05f; i += 0.01f)
        {
            for (float j = -0.05f; j < 0.05f; j += 0.01f)
            {
                GL.Vertex3(a.x + i, a.y + j, 0);
                GL.Vertex3(b.x + i, b.y + j, 0);
            }
        }

        GL.End();
    }

    // convert "binary" to int (0 0 0 0 = 0)
    // i.e. return the configuration ID
    int GetState(float a, float b, float c, float d)
    {
        return isInsideSI(a) * 8 + isInsideSI(b) * 4 + isInsideSI(c) * 2 + isInsideSI(d) * 1;
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
        return falloff((centre - p).sqrMagnitude, radius*radius);
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

        return v - 0.5f;
    }

    public int isBlobSI(Vector2 p)
    {
        float si = SI(p);

        if (si >= 0f)
            return 1;

        return 0;
    }

    public int isInsideSI(float v)
    {
        if (v >= 0f)
            return 1;

        return 0;
    }

    // Esimates linearly the surface parametric position
    // return the parametric distance from a to b
    // Default : 0
    public float SurfaceParametricEstimation(float va, float vb)
    {
        return Mathf.Abs(va) / (Mathf.Abs(va) + Mathf.Abs(vb));
    }

    // Estimate linearly the position where the surface crosses the [A, B] segment
    // Default : A
    public Vector2 SurfacePositionEstimation(Vector2 A, float va, Vector2 B, float vb)
    {
        return A + (B - A) * SurfaceParametricEstimation(va, vb);
    }

    public Vector2 GridToWorld(int x, int y)
    {
        return new Vector2(blob_bb.bottom_left_corner.x + x * square_size, blob_bb.bottom_left_corner.y + y * square_size);
    }
}
