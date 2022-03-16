using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision otherObj)
    {
        if (otherObj.gameObject.tag == "Player")
        {
            Destroy(otherObj.gameObject, .0f);
        }
    }
}
