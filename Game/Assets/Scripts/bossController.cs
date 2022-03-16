using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossController : MonoBehaviour
{
    private Animator anim;
    public playerController pc;
    public int health = 6;
    private bool getHit, end, first;
    public bool dead, attack;
    private int insectsNum;

    public AudioClip dyingSound;
    private AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        first = true;
        getHit = attack = end = dead = false;
        insectsNum = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
        {
            dead = true;
        }
        if (!dead)
        {
            insectsNum = pc.insectsNum;
            if (end)
            {

                if (!getHit)
                {
                    health -= insectsNum;
                    getHit = true;
                }

                if (first && insectsNum > 0)
                {
                    first = false;
                    StartCoroutine("WaitHits");
                }

            }
            else if (pc.end)
            {
                anim.SetInteger("AnimId", 1);
                end = true;
            }
        }
    }

    IEnumerator WaitHits()
    {
        anim.SetInteger("AnimId", 2);
        yield return new WaitForSeconds(5);
            StartCoroutine("WaitAttack");
    }

    IEnumerator WaitAttack()
    {
        if (health <= 0 || pc.invincible) dead = true;
        if (!dead)
        {
            anim.SetInteger("AnimId", 3);
            attack = true;
            yield return new WaitForSeconds(5);
            anim.SetInteger("AnimId", 1);
        }
        else
        {
            audioS.PlayOneShot(dyingSound);
            anim.SetBool("Dead", true);
        }
    }


}