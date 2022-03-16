using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shurikenMove : MonoBehaviour
{
    public bool right;
    public float speed = 0.05f;
    private AudioSource audioS;
    public AudioClip effect;

    // Start is called before the first frame update
    void Start()
    {
        right = true;
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuController.inMenu)
        {
            if (transform.position.x >= 23.0f) right = false;
            else if (transform.position.x <= -17.5f) right = true;
            if (right) transform.position += new Vector3(speed, 0, 0);
            else transform.position -= new Vector3(speed, 0, 0);
        }
    }

    void OnCollisionEnter(Collision obj)
    {
        if(obj.gameObject.tag == "Player" || obj.gameObject.tag == "insect")
        {
            audioS.PlayOneShot(effect, 0.5f);
        }
    }


}
