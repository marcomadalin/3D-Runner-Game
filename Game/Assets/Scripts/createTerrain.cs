using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createTerrain : MonoBehaviour
{
    public GameObject father;
    public int nrepetitions = 30;
    public float displacementz = 40.0f;
    public float positionx = 0.0f;
    public float positiony = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        for (int z = 0; z < nrepetitions; ++z)
        {
            GameObject obj = (GameObject)Instantiate(father, new Vector3(positionx, positiony, z * displacementz), father.transform.rotation);
            obj.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
