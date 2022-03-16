using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPlayer : MonoBehaviour
{
    public GameObject player;
    private playerController pc;
    private bool first;
        // Start is called before the first frame update
    void Start()
    {
        first = true;
        pc = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc != null && pc.end)
        {
            if (first)
            {
                transform.Rotate(-38, 0, 0);
                first = false;
                transform.position = new Vector3(0, 10, transform.position.z + 7);
            }
        }
        else if (pc != null) transform.position = player.transform.position + new Vector3(0, 8, -11);
    }
}
