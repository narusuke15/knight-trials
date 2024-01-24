using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour, IHitable, IWakable
{
    public int HP = 10;
    public float MoveSpeed = 0.1f;
    public bool Alert = false;
    public ParticleSystem ParticleSystem;
    public GameObject HitParticle;

    private int hDirection = 1;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool moving = false;
    bool isHit = false;
    int flashCount = 0;
    private Transform playerTransform;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Alert)
        {
            var deltaPos = (playerTransform.position - transform.position).normalized;
            if (deltaPos.x > 0)
            {
                hDirection = 1;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (hDirection != -1)
            {
                hDirection = -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            transform.Translate(deltaPos * MoveSpeed * Time.deltaTime);
        }
    }

    public void SetAlert()
    {
        Alert = true;
    }

    public void WakeDelay()
    {
        Invoke("SetAlert", 0.5f);
        animator.SetBool("Alert", true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Weapon")
        {
            if (other.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Knife_Attack"))
            {
                GetComponent<Rigidbody2D>().AddForce(transform.position - other.transform.position);
                Hit();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // GetComponent<Rigidbody2D>().AddForce(transform.position - other.transform.position);
            Alert = false;
            animator.SetBool("Alert", false);
            CancelInvoke();
            Invoke("WakeDelay", 0.5f);
        }
    }

    public void Hit()
    {
        if (isHit)
            return;

        Instantiate(HitParticle, transform.position, transform.rotation);
        HP--;
        if (HP <= 0)
        {
            ParticleSystem.gameObject.SetActive(true);
            ParticleSystem.transform.SetParent(null);
            Destroy(gameObject);
        }
        else
        {
            flashCount = 0;
            isHit = true;
            FlashOn();
        }
    }

    public void FlashOn()
    {
        flashCount++;
        spriteRenderer.material.SetFloat("_FlashAmount", 1);
        Invoke("FlashOff", 0.2f);
    }

    public void FlashOff()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        if (flashCount < 1)
            Invoke("FlashOn", 0.2f);
        else
            isHit = false;
    }

    public void Wake(Transform target)
    {
        Invoke("SetAlert", 0.5f);
        animator.SetBool("Alert", true);
        playerTransform = target;
    }

    public void Sleep()
    {
        // Alert = false;
        // animator.SetBool("Alert", false);
        // playerTransform = null;
    }
}
