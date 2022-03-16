using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;

    private AudioSource audioS;
    [SerializeField] private AudioSource running;
    public AudioClip jumpingSound, explosion;

    public GameObject terrainParticles, deatheffect;
    private GameObject particles;

    public int insectsNum;
    public float speed = 7;
    public float velocityLimit = 25.0f;
    private float hor, vert;
    public float jumpPower = 7.0f;
    public bool canJump, dead, end, ini, bossWait, invincible;
    public Vector3 velocity;

    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private LevelController levelController = null;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] bossController bc;

    void Start()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds)) playerInput.actions.LoadBindingOverridesFromJson(rebinds);

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        end = ini = canJump = dead = bossWait = invincible = false;
        vert = 1.0f;
        insectsNum = 0;
        running.Play();
    }

    void FixedUpdate()
    {
        if (!dead && ini && !end)
        {
            Vector3 direction = new Vector3(hor * (speed + 5), 0.0f, vert * speed);
            rb.AddForce(direction);
            if (rb.velocity.magnitude > velocityLimit) rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityLimit);
        }
    }

    void Update()
    {
        velocity = rb.velocity;
        if (ini && !end && !dead)
        {
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

        if (bossWait)
        {
            bossWait = false;
            if (bc.dead) levelController.LevelComplete();
            else levelController.GameOverScreen();
        }
    }

    IEnumerator StartBoss()
    {
        yield return new WaitForSeconds(7.0f);
        bossWait = true;
    }

    public void PauseWalkSound()
    {
        if (running.isPlaying) running.Stop();
    }

    public void ResumeWalkSound()
    {
        if (!running.isPlaying && canJump) running.Play();
    }

    void OnCollisionEnter(Collision obj)
    {

        if (!ini && obj.gameObject.tag == "Floor")
        {
            ini = true;
            anim.SetBool("inFloor", true);
            particles = Instantiate(terrainParticles, transform.position, transform.rotation);
            canJump = true;

        }
 
        if (!dead && obj.gameObject.tag == "Barrier")
        {
            Instantiate(deatheffect, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
            dead = true;

            running.Stop();
            audioS.PlayOneShot(explosion);
            Destroy(transform.gameObject, 0.3f);
            Destroy(particles);
            levelController.GameOverScreen();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            end = true;
            rb.velocity = new Vector3(0, 0, 0);
            anim.SetBool("inFloor", false);
            anim.SetBool("Jumping", false);
            running.Stop();
            StartCoroutine(StartBoss());
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
         hor = ctx.ReadValue<float>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (ini && !dead && !end)
        {
            if (canJump)
            {
                canJump = false;
                audioS.PlayOneShot(jumpingSound);
                anim.SetBool("Jumping", true);
                rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            }
            else
            {
                anim.SetBool("inFloor", false);
                anim.SetBool("Jumping", false);
            }
        }
    }
}
