using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubulles : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab_bulle;
    public GameObject prefab_bulle_eclatee;

    List<GameObject> bulles = new List<GameObject>();

    public Vector2 offset_spawn = new Vector2();
    public Vector2 range_spawn = new Vector2();

    // spawn bulle tout les delta_t
    public float delta_t;
    public float last_time;

    void Start()
    {
        last_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (last_time - delta_t <= 0)
        {
            spawn_bubble();
            last_time = Time.time;
        }
    }

    void spawn_bubble()
    {

        float x = Mathf.Lerp(-range_spawn.x, range_spawn.x, Random.value);
        float y = Mathf.Lerp(-range_spawn.y, range_spawn.y, Random.value);
        GameObject go = Instantiate(prefab_bulle, offset_spawn + new Vector2(x, y), Quaternion.identity, this.transform);
        bulles.Add(go);
    }
}
