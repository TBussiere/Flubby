using System.Collections;
using System.Collections.Generic;


public class Node
{
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
        other.neighboors.Remove(this);
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

    public HashSet<Node> GetNeighboors()
    {
        return neighboors;
    }
}
