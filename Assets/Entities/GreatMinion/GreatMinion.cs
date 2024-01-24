using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatMinion : MonoBehaviour, IHitable, IWakable
{
    public int HP = 10;
    public float MoveSpeed = 0.1f;
    public bool Alert = false;
    public ParticleSystem ParticleSystem;
    public GameObject HitParticle;
    public GameObject MinionProjectile;

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
        InvokeRepeating("ThrowTick", 2f, 2f);
    }

    void Update()
    {
        if (!Alert)
            return;

        if (!playerTransform)
            return;

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
    }

    public void Wake(Transform target)
    {
        Invoke("SetAlert", 0.5f);
        animator.SetBool("Alert", true);
        playerTransform = target;
    }

    public void Sleep()
    {
        Alert = false;
        playerTransform = null;
    }

    public void SetAlert()
    {
        Alert = true;
    }

    void ThrowTick()
    {
        if (!Alert)
            return;

        moving = true;
        var chance = Random.Range(0, 3);
        if (chance != 0)
        {
            animator.SetTrigger("ThrowMinion");
        }
    }
   
    public void ThrowMinion()
    {
        var minion = Instantiate(MinionProjectile, transform.position - (new Vector3(0.3f, 0, 0) * hDirection), transform.rotation) as GameObject;
        minion.transform.localScale = new Vector3(hDirection, 1, 1);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isHit)
            return;

        if (other.tag == "Weapon")
        {
            if (other.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Knife_Attack"))
            {
                transform.Translate((transform.position - other.transform.position).normalized.x * 0.1f, 0, 0, Space.World);
                Hit();
            }
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
            ThrowMinion(); // throw minion before death
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
