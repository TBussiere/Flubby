using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSpring : MonoBehaviour
{
    // nombre de particule
    public int nb_particule = 40;

    // Masse globale du système répartie équitablement entre les noeuds
    public float mass = -1;

    // Radius de la sphere d'une particule
    public float radius = 1;

    // fréquence des noeuds
    public float frequency;

    // damping ratio
    public float damping;

    public List<Node> nodes = new List<Node>();

    public MassSpring()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                nodes.Add(new Node(radius, 1, 0.9f, 0.9f));
        
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
            {
                
            }
    }
}
