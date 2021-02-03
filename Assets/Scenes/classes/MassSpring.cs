using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSpring : MonoBehaviour
{
    // nombre de particule
    public int nb_particule = 40;

    // Masse globale du système répartie équitablement entre les noeuds
    public float mass = 40;

    // Radius de la sphere d'une particule
    public float radius = 1;

    // fréquence des noeuds
    public float frequency = 1;

    // damping ratio
    public float damping = 0.1f;

    public float spring_length = 0.5f;

    public List<Node> nodes = new List<Node>();

    public MassSpring()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                nodes.Add(new Node(radius, 1, 0.9f, 0.9f));

        // pour chaque noeud
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                Node current_node = nodes[i * 4 + j];
                // je crée les ressorts entre ce noeud et ceux dans sa 8 adjacence
                for (int ip = -1; ip < 2; ip++)
                    for (int jp = -1; jp < 2; jp++)
                    {
                        int row = i + ip;
                        int col = j + jp;

                        if (row < 0 || row >= 4 || col < 0 || col >= 4)
                            continue;
                        Node looking = nodes[row * 4 + col];
                        if (current_node.isAlreadyConnectedTo(looking))
                            continue;

                        create_spring(current_node, looking);
                    }
            }
    }

    SpringJoint2D create_spring(Node left, Node right)
    {
        SpringJoint2D spring = left.sphere.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
        spring.enableCollision = true;
        spring.distance = spring_length;
        spring.dampingRatio = damping;
        spring.frequency = frequency;
        spring.connectedBody = right.sphere.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;

        left.link(right, spring);
        right.link(left, spring);

        return spring;
    }
}
