using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    public bool v1_to_v2;
    public bool v2_to_v1;
    public PlayerInsideCollider v1;
    public PlayerInsideCollider v2;
    public GameObject cover_v1;
    public GameObject cover_v2;

    public CameraMouvement cm;

    // Update is called once per frame
    void Update()
    {
        Compute();
    }

    void Compute()
    {
        if (!v1.PlayerInside && !v2.PlayerInside)
        {
            v1_to_v2 = false;
            v2_to_v1 = false;
            return;
        }
        else if (!v1.PlayerInside && v2.PlayerInside)
        {
            if (v1_to_v2)
            {
                cm.SwitchView();
                cover_v1.SetActive(true);
            }
            else
            {
                cover_v1.SetActive(true);
                v2_to_v1 = true;
            }
        }
        else if (v1.PlayerInside && !v2.PlayerInside)
        {
            if (v2_to_v1)
            {
                cm.SwitchView();
                cover_v2.SetActive(true);
            }
            else
            {
                cover_v2.SetActive(true);
                v1_to_v2 = true;
            }
        }
        else
        {
            cover_v1.SetActive(false);
            cover_v2.SetActive(false);
        }
    }
}
