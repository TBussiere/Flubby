using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public GameObject sphere;

    /*
     * Liste des spring, le spring à l'indice i relie le Node *this* au node
     * dans linked_nodes au meme indice, c'est-à-dire à l'indice i.
     */
    public List<SpringJoint2D> springs = new List<SpringJoint2D>();
    public List<Node> linked_nodes = new List<Node>();

    public Node(float radius, float mass, float drag, float angularDrag)
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        CircleCollider2D collider = sphere.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        collider.radius = radius;

        //https://www.reddit.com/r/Unity2D/comments/ki7a7b/friction_not_working_on_circle_collider_2d/
        Rigidbody2D rigidbody = sphere.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        rigidbody.mass = mass;
        rigidbody.drag = drag;
        rigidbody.angularDrag = angularDrag;

    }

    public bool isAlreadyConnectedTo(Node node)
    {
        foreach (Node n in linked_nodes)
            if (n.sphere == node.sphere)
                return true;

        return false;
    }

    public void link(Node other, SpringJoint2D spring)
    {
        springs.Add(spring);
        linked_nodes.Add(other);
    }

    /* 
     * Dans unlink, on supprime le spring et le node associé dans la liste,
     * et on appelle unlink sur le node associé
     */
    public void unlink(SpringJoint2D spring)
    {
        int idx = -1;
        for (int i = 0; i < springs.Count; i++)
            if (spring == springs[i])
            {
                idx = i;
                break;
            }

        if (idx != -1)
        {
            springs.RemoveAt(idx);
            linked_nodes[idx].unlink(spring);
            linked_nodes.RemoveAt(idx);
            Destroy(spring);
        }
    }

}
