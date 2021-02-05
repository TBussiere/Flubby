using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobView : MonoBehaviour
{
    
    public BlobController controller;

    public GameObject prefab_particule;
    // nombre de particule
    // public int nb_particule = 40;

    // Masse globale du système répartie équitablement entre les noeuds
    // public float mass = 40;

    // Radius de la sphere d'une particule
    // public float radius = 1;

    // fréquence des noeuds
    public float frequency = 1;

    // damping ratio
    public float damping = 0.1f;

    public float spring_length = 0.5f;

    public List<GameObject> particules = new List<GameObject>();
    // public List<List<SpringJoint2D>> springs_of_spheres = new List<List<SpringJoint2D>>();


    // Accesseurs
    // on ne garde que des indices, pas des references
    // node => GameObject Sphere / Particules
    // indice node dans model => indice Particule dans `particules`
    public Dictionary<int, int> nodeToParticule = new Dictionary<int, int>();
    // GameObject Sphere / Particules => node
    // InstanceID GameObject Particule => indice node dans model
    public Dictionary<int, int> ParticuleToNode = new Dictionary<int, int>();


    void Start()
    {
        
        Graph model = controller.model;
        for (int i = 0; i < model.Count(); i++)
        {
            GameObject particule = Instantiate(prefab_particule, controller.transform);
            particules.Add(particule);

            // init des accesseurs
            nodeToParticule.Add(i, i);
            ParticuleToNode.Add(particule.GetInstanceID(), i);
        }

        for (int i = 0; i < model.Count(); i++)
        {
            Node n = model.GetNode(i);
            foreach (Node neighboor in n.neighboors)
            {
                // hack en gros, vu qu'on crée en grille les Node puis les edges, le node j qui
                // a un indice plus grand qu'un autre Node i , si i et j on une arete qui les relie, alors dans
                // le parcourt et la creation des ressorts, on a déjà créé le ressort qui va de i à j, 
                // donc pas besoin de faire l'inverse
                // voir si ca fonctionne avec une autre impl
                if (neighboor.getIdentityID() > n.getIdentityID())
                    continue;
                create_spring(n, neighboor);
            }
        }
    }

    void Update()
    {

    }

    SpringJoint2D create_spring(Node left, Node right)
    {
        int idx_left_particule = nodeToParticule[left.getIdentityID()];
        GameObject left_particule = particules[idx_left_particule];
        int idx_right_particule = nodeToParticule[right.getIdentityID()];
        GameObject right_particule = particules[idx_right_particule];

        SpringJoint2D spring = left_particule.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
        spring.enableCollision = true;
        spring.distance = spring_length;
        spring.dampingRatio = damping;
        spring.frequency = frequency;
        spring.autoConfigureDistance = false;
        spring.connectedBody = right_particule.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        return spring;
    }
}
