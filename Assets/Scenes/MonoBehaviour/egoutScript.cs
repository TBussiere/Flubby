using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class egoutScript : MonoBehaviour
{
    public MarchesHandler marchesHandler;
    public BlobController bc;
    public CameraMouvement camera;

    int nbPartPass = 0;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(marchesHandler, "Script egoutScript needs ref to MarchesHandler. obj : " + this.name);
        UnityEngine.Assertions.Assert.IsNotNull(bc, "Script egoutScript needs ref to BlobController. obj : " + this.name);
        UnityEngine.Assertions.Assert.IsNotNull(camera, "Script egoutScript needs ref to CameraMouvement. obj : " + this.name);
    }

    private void Update()
    {
        if (nbPartPass >= bc.view.particules.Count/2)
        {
            marchesHandler.timeUntilHand += 10;
            marchesHandler.stopAlarm();
            camera.enabled = false;
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            nbPartPass++;
        }
    }
}
