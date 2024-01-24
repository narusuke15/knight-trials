using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatWizard : MonoBehaviour
{
    public int HP = 10;
    public float AttackRate = 4f;
    public float MoveSpeed = 0.1f;
    public GameObject FireBall;
    public ParticleSystem DeadParticleSystem;
    public GameObject HitParticle;

    private int hDirection = 1;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool moving = false;
    bool isHit = false;
    int flashCount = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("ShootFireBall", AttackRate, AttackRate);
    }

    void ShootFireBall()
    {
        var fireball = Instantiate(FireBall, transform.position + new Vector3(0.1f, 0.5f, -0.1f), transform.rotation) as GameObject;
        fireball.transform.localScale = transform.localScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log("skeleton is hit! : " + other.gameObject.name);
        if (other.tag == "Weapon")
        {
            if (other.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Knife_Attack"))
                Hit();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            Hit();
            Destroy(other.gameObject);
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
            DeadParticleSystem.gameObject.SetActive(true);
            DeadParticleSystem.transform.SetParent(null);
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
