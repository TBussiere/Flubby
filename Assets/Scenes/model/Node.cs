using System.Collections;
using System.Collections.Generic;


public class Node
{
    // public GameObject sphere;

    int identity_idx;

    public HashSet<Node> neighboors = new HashSet<Node>();
    
    /*
     * Liste des spring, le spring à l'indice i relie le Node *this* au node
     * dans linked_nodes au meme indice, c'est-à-dire à l'indice i.
     */
    // public List<SpringJoint2D> springs = new List<SpringJoint2D>();
    // public List<Node> linked_nodes = new List<Node>();

    public Node(int idx)
    {
        
        // sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere) as GameObject;
        // // Destroy(sphere.GetComponent(typeof(SphereCollider)));
        // // Instantiate( );
        // CircleCollider2D collider = sphere.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        // collider.radius = radius;

        // //https://www.reddit.com/r/Unity2D/comments/ki7a7b/friction_not_working_on_circle_collider_2d/
        // Rigidbody2D rigidbody = sphere.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        // rigidbody.mass = mass;
        // rigidbody.drag = drag;
        // rigidbody.angularDrag = angularDrag;
        identity_idx = idx;
    }

    public bool isAlreadyConnectedTo(Node node)
    {

        if (neighboors.Contains(node))
            return true;

        return false;
    }

    public void create_edge(Node other)
    {
        neighboors.Add(other);
    }

    public void delete_edge(Node other)
    {
        other.delete_edge(this);
        neighboors.Remove(other);
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        Node n = obj as Node;
        return n.identity_idx == identity_idx;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return identity_idx.GetHashCode();
    }

    public int Count()
    {
        return neighboors.Count;
    }

    public int getIdentityID()
    {
        return identity_idx;
    }
}
