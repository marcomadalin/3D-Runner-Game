using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class insectController : MonoBehaviour
{
    public Vector3[] positionMatrix = {new Vector3(0,1,-2),new Vector3(-2, 0, 0),new Vector3(2, 0, 0) ,new Vector3(0, 0, 2), new Vector3(-2,0,-2), new Vector3(2, 0, -2), new Vector3(-2, 0, 2),new Vector3(2, 0, 2),
                                        new Vector3(0,0,-4),new Vector3(-4, 0, 0),new Vector3(4, 0, 0) ,new Vector3(0, 0, 4),new Vector3(-2,0,-4), new Vector3(2,0,-4), new Vector3(-4,0,-2), new Vector3(4,0,-2),
                                        new Vector3(-4,0,2),new Vector3(4, 0, 2),new Vector3(-2, 0, 4) ,new Vector3(2, 0, 4), new Vector3(-4,0,-4), new Vector3(4,0,-4), new Vector3(-4,0,4), new Vector3(4,0,4)
                                        };


    private playerController pc;
    public bossController bc;
    public GameObject astronaut, EndWall;

    private Animator anim;
    private Rigidbody rb;

    public GameObject effect, terrainParticles, magic;
    private GameObject mEff, particles;

    private AudioSource audioS;
    public AudioClip dying, increase;
    public AudioClip collect;

    public bool canJump, dead, end, ini, inBoss, endfight, inWall;
    public float jumpPower, speed;
    public Vector3 velocity;
    public int id;

    [SerializeField] private PlayerInput insectInput = null;
    public PlayerInput InsectInput => insectInput;

    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;


    // Start is called before the first frame update
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
        pc = astronaut.GetComponent<playerController>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        id = -1;
        ini = canJump = dead = end = inWall = endfight = false;
    }

    void FixedUpdate()
    {
        if (!dead && ini && !end)
        {
            velocity = pc.velocity;
            rb.velocity = velocity;
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);

        }

    }

    void Update()
    {
        if (bc.dead)
        {
            Destroy(particles);
            Destroy(mEff, 5.0f);

        }
        else if (bc.attack && !dead)
        {

            dead = true;
            anim.SetBool("Attack", false);
            anim.SetBool("Dead", true);
            gameObject.layer = 7;
            Destroy(transform.gameObject, 5.0f);
            Destroy(particles);

            StartCoroutine("Wait");

            //Acabar nivel

        }
        if (pc.end)
        {
            end = true;
            anim.SetBool("Following", false);
            anim.SetBool("inFloor", false);
            anim.SetBool("Jumping", false);

        }
        if (end && !dead && ini && !endfight)
        {
            endfight = true;
            float xpos = -8.0f + id;
            transform.position = new Vector3(xpos, 0.05f, EndWall.transform.position.z + 15);
            anim.SetBool("Attack", true);
            mEff = Instantiate(magic, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), magic.transform.rotation);
        }
        if (ini && !dead && !end)
        {
            float dist = DistancetoAstronaut();
            if (dist > 0.1 && !inWall && transform.position.x > -4.0f && transform.position.x < 15.0f) transform.position = astronaut.transform.position + positionMatrix[id];
            speed = pc.speed;
            jumpPower = pc.jumpPower;
            canJump = pc.canJump;
            if (canJump)
            {
                particles.transform.position = transform.position;
                anim.SetBool("inFloor", true);
            }
            else
            {
                anim.SetBool("inFloor", false);
                anim.SetBool("Jumping", false);

            }
        }

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(mEff);
        audioS.PlayOneShot(dying, 0.1f);
    }

    void OnCollisionExit(Collision obj)
    {

        if (obj.gameObject.tag == "Wall")
        {
           inWall = false;
        }

    }

    void OnCollisionEnter(Collision obj)
    {
        if (!ini && obj.gameObject.tag == "Floor")
        {
            anim.SetBool("inFloor", true);
            canJump = true;
        }

        if (obj.gameObject.tag == "Barrier")
        {
            if (!dead)
            {
                pc.insectsNum -= 1;
            }
            gameObject.layer = 7;
            Destroy(transform.gameObject, 5.0f);
            Destroy(particles);
            audioS.PlayOneShot(dying, 0.2f);
            dead = true;
            anim.SetBool("Dead", true);
        }

        if (obj.gameObject.tag == "Wall")
        {
            inWall = true;
        }

    }


    
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Boss")
        {
            inBoss = true;
        }
        if (obj.gameObject.tag == "Player")
        {
            if (!ini)
            {
                audioS.PlayOneShot(collect);
                GameObject eff = Instantiate(effect, transform.position, transform.rotation);
                Destroy(eff, 5.0f);
                particles = Instantiate(terrainParticles, transform.position, transform.rotation);
                transform.rotation = new Quaternion(0,0,0,0);
                pc.insectsNum += 1;

                GameObject dad = transform.parent.gameObject;
                insectFather iF = dad.GetComponent<insectFather>();
                id = iF.getFirstId();
                transform.position = astronaut.transform.position + positionMatrix[id];
                anim.SetBool("Following", true);

            }
            ini = true;
        }
    }

    float DistancetoAstronaut()
    {
        if (!pc.dead) 
        {
            float dist = Vector3.Distance(astronaut.transform.position, transform.position);
            Vector3 avg = positionMatrix[id];
            float correct;
            if (avg.x != 0) correct = (float)avg.x;
            else correct = (float)avg.z;
            if (correct < 0) correct = (-1) * correct;
            float res = dist - correct;
            if (res > 0) return res;
            else return (-res);
        }
        return 0;
    }

    public void increaseEffect()
    {

        audioS.PlayOneShot(increase, 0.5f);
        GameObject eff = Instantiate(effect, transform.position, transform.rotation);
        Destroy(eff, 5.0f);

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (ini && !dead && !end)
        {
            canJump = pc.canJump;
            if (canJump)
            {
                particles.transform.position = transform.position;
                anim.SetBool("inFloor", true);
            }
            else
            {
                anim.SetBool("inFloor", false);
                anim.SetBool("Jumping", false);
            }
        }
    }
}
