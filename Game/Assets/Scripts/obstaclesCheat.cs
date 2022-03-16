using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstaclesCheat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void invencible()
    {
        int n = transform.childCount;
        for (int i = 0; i < n; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == "Barrier") child.layer = 8;

            for (int j = 0; j < child.transform.childCount; ++j)
            {
                GameObject child2 = child.transform.GetChild(j).gameObject;
                if (child2.tag == "Barrier") child2.layer = 8;
            }
        }
    }
    public void vencible()
    {
        int n = transform.childCount;
        for (int i = 0; i < n; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == "Barrier") child.layer = 0;

            for (int j = 0; j < child.transform.childCount; ++j)
            {
                GameObject child2 = child.transform.GetChild(j).gameObject;
                if (child2.tag == "Barrier") child2.layer = 0;
            }
        }
    }
}
