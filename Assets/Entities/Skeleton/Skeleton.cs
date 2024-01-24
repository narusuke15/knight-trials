using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IHitable
{
    public int HP = 10;
    public float MoveSpeed = 0.1f;
    public ParticleSystem ParticleSystem;
    public GameObject HitParticle;

    private int hDirection = 1;
    private int vDirection = 0;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool moving = false;
    bool isHit = false;
    int flashCount = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("MoveTick", 2, 2);
    }

    void Update()
    {
        if (hDirection != 0)
            transform.Translate(hDirection * MoveSpeed * Time.deltaTime, vDirection * MoveSpeed * Time.deltaTime, 0);
    }

    void MoveTick()
    {
        moving = true;
        hDirection = Random.Range(-1, 2);
        vDirection = Random.Range(-1, 2);
        if (hDirection != 0)
        {
            animator.SetBool("Moving", true);
            transform.localScale = new Vector3(hDirection, 1, 1);
        }
        else
            animator.SetBool("Moving", false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isHit)
            return;
        // Debug.Log("skeleton is hit! : " + other.gameObject.name);
        if (other.tag == "Weapon")
        {
            if (other.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Knife_Attack"))
            {
                transform.Translate((transform.position - other.transform.position).normalized.x * 0.15f, 0, 0, Space.World);
                // transform.Translate(Vector2.one * transform.localScale.x * 0.2f);
                Hit();
            }
        }
    }

    public void Hit()
    {
        if (isHit)
            return;
        moving = false;
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
}
