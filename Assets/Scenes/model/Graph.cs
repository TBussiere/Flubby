using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Node> nodes = new List<Node>();

    public Graph()
    {
        // Pour l'instant, on crée une grille
        int length = 4;
        int idx = 0;
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

    int index(int row, int col, int length)
    {
        return row * length + col;
    }


}
