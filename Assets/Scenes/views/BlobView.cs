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
    public float spring_length = 0.7f;
    public float max_distance_joint = 1f;
    // public float radius = 0.5f;
    // public float collider_radius = 0.5f;

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
            KeyValuePair<int, int> pos = model.inverse_index(i);
            Vector3 position = new Vector3(pos.Key * spring_length, pos.Value * spring_length, 0) + controller.transform.position;
            GameObject particule = create_particle(position);
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

                int idx_n_particule = nodeToParticule[n.getIdentityID()];
                GameObject left_particule = particules[idx_n_particule];
                int idx_neighboor_particule = nodeToParticule[neighboor.getIdentityID()];
                GameObject right_particule = particules[idx_neighboor_particule];

                create_spring(left_particule, right_particule);
                create_distance_joint(left_particule, right_particule);
            }
        }
    }

    void Update()
    {
        // Suppression de liaisons
        // 1) Faire une fonction de suppresion d'un spring
        // 2) Liste des voisins ayant plus de n liaisons
        // 3) Distance max dans cette sous-sélection


        // Creation de liaisons
        // -> Ailleurs ? (dans le contrôlleur de la particule elle-même)
    }

    GameObject create_particle(Vector3 position)
    {
        GameObject particule = Instantiate(prefab_particule, position, Quaternion.identity, controller.transform);
        particule.tag = "BlobParticule";
        // CircleCollider2D collider = particule.GetComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        // collider.radius = collider_radius;
        // particule.transform.localScale = new Vector2(radius, radius);
        return particule;
    }

    SpringJoint2D create_spring(GameObject left_particule, GameObject right_particule)
    {
        SpringJoint2D spring = left_particule.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
        spring.enableCollision = true;
        spring.autoConfigureDistance = false;
        spring.distance = spring_length;
        spring.dampingRatio = damping;
        spring.frequency = frequency;
        spring.connectedBody = right_particule.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        return spring;
    }

    DistanceJoint2D create_distance_joint(GameObject left_particule, GameObject right_particule)
    {
        DistanceJoint2D joint = left_particule.AddComponent(typeof(DistanceJoint2D)) as DistanceJoint2D;
        joint.maxDistanceOnly = true;
        joint.distance = max_distance_joint;
        joint.autoConfigureDistance = false;
        joint.enableCollision = true;
        joint.connectedBody = right_particule.GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        return joint;
    }

    void remove_spring(Node left, Node right)
    {
        int idx_left_particule = nodeToParticule[left.getIdentityID()];
        GameObject left_particule = particules[idx_left_particule];
        int idx_right_particule = nodeToParticule[right.getIdentityID()];
        GameObject right_particule = particules[idx_right_particule];

        // Cherche si le ressort est de gauche à droite.
        Component[] springs_left_to_right;
        springs_left_to_right = left_particule.GetComponents(typeof(HingeJoint));

        foreach (SpringJoint2D joint in springs_left_to_right)
        {
            if (joint.attachedRigidbody.gameObject == right_particule)
            {
                Destroy(joint);
            }
        }

        // Cherche si le ressort est de droite à gauche. (on le fait au cas où deux springs aient été mis)
        Component[] springs_right_to_left;
        springs_right_to_left = right_particule.GetComponents(typeof(HingeJoint));

        foreach (SpringJoint2D joint in springs_right_to_left)
        {
            if (joint.attachedRigidbody.gameObject == left_particule)
            {
                Destroy(joint);
            }
        }
    }

    float distance(Node left, Node right)
    {
        int idx_left_particule = nodeToParticule[left.getIdentityID()];
        GameObject left_particule = particules[idx_left_particule];
        int idx_right_particule = nodeToParticule[right.getIdentityID()];
        GameObject right_particule = particules[idx_right_particule];

        Vector2 l2 = left_particule.transform.position;
        Vector2 r2 = right_particule.transform.position;
        return (l2 - r2).magnitude;
    }

    /**
     * break_distance : min distance to break a link between two particles
     * n : min amount of neighboors to kill a link
     */
    void break_particles_links(float break_distance, int n)
    {
        Graph model = controller.model;

        // NON ! -> tester à chaque fois ...
        foreach (Node node_ in model.GetNodesWithMinNeighbors(n))
        {
            float max_dist = 0;
            Node further_node = null;

            foreach (Node neighboor_ in model.GetBreakableNeighboors(node_, n))
            {
                float dist = distance(node_, neighboor_);
                if (dist > max_dist)
                {
                    max_dist = dist;
                    further_node = neighboor_;
                }
            }

            if (further_node != null)
            {
                // model.remove_link();
                remove_spring(node_, further_node);
            }
        }
    }
}
