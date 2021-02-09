using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Node> nodes = new List<Node>();
    int length = 7;
    int idx = 0;
    public Graph()
    {
        // Pour l'instant, on crée une grille
        for (int i = 0; i < length; i++)
            for (int j = 0; j < length; j++)
            {
                nodes.Add(new Node(idx));
                idx++;
            }

        // pour chaque noeud
        for (int i = 0; i < length; i++)
            for (int j = 0; j < length; j++)
            {
                Node current_node = nodes[index(i, j, length)];

                for (int ip = -1; ip < 2; ip++)
                    for (int jp = -1; jp < 2; jp++)
                    {
                        if (ip == 0 && jp == 0)
                            continue;

                        int row = i + ip;
                        int col = j + jp;

                        if (row < 0 || row >= length || col < 0 || col >= length)
                            continue;

                        Node other = nodes[index(row, col, length)];
                        if (current_node.isAlreadyConnectedTo(other))
                            continue;

                        current_node.create_edge(other);
                        other.create_edge(current_node);
                    }
            }
    }
    
    public int Count()
    {
        return nodes.Count;
    }

    public Node GetNode(int idx)
    {
        return nodes[idx];
    }

    public List<Node> GetNodes()
    {
        return nodes;
    }

    public int index(int row, int col, int length)
    {
        return row * length + col;
    }

    // ATTENTION 
    // ne fonctionne que si systeme de grille, voir pour autre
    public KeyValuePair<int, int> inverse_index(int x)
    {
        return new KeyValuePair<int, int>(x / length, x % length);
    }

    public List<Node> GetNodesWithMinNeighbors(int n)
    {
        List<Node> eligibles = new List<Node>();
        foreach (Node node_ in nodes)
        {
            if (node_.Count() >= n)
            {
                eligibles.Add(node_);
            }
        }

        return eligibles;
    }

    public List<Node> GetBreakableNeighboors(Node node_, int n)
    {
        HashSet<Node> neighboors = node_.GetNeighboors();
        List<Node> breakable = new List<Node>();
            
        foreach (Node neighboor_ in neighboors)
        {
            if (neighboor_.Count() >= n)
            {
                breakable.Add(neighboor_);
            }
        }

        return breakable;
    }

    /*
    public List<Node> GetBreakableNeighboors(Node node_, int n)
    {
        List<Node> eligibles = GetNodesWithMinNeighbors(n);
        List<Node> breakable = new List<Node>();

        foreach (Node node_ in eligibles)
        {
            HashSet<Node> neighboors = node_.GetNeighboors();
            bool has_breakable_link = false;
            
            foreach (Node neighboor_ in neighboors)
            {
                if (neighboor_.Count() >= n)
                {
                    has_breakable_link = true;
                    break;
                }
            }

            if (has_breakable_link)
            {
                breakable.Add(node_);
            }
        }

        return breakable;
    }
    */
}
