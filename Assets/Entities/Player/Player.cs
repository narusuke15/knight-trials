using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitable
{
    public int HP = 10;
    public float MoveSpeed = 0.1f;
    public GameObject Weapon;
    public GameObject WalkParticle;

    private int hDirection = 1;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool moving = false;
    bool isHit = false;
    bool isDead = false;
    int flashCount = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("WalkEffect", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        moving = false;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0);
            hDirection = -1;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
            hDirection = 1;
            moving = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, MoveSpeed * Time.deltaTime, 0);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -MoveSpeed * Time.deltaTime, 0);
            moving = true;
        }

        //flip
        if (hDirection < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {

            animator.SetBool("Move", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) ||
                   Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (!moving)
                animator.SetBool("Move", false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
            Weapon.GetComponent<Animator>().SetTrigger("Attack");
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isHit)
            return;
        if (other.gameObject.tag == "Enemy")
        {
            Hit();
        }

        if (other.gameObject.tag == "EnemyProjectile")
        {
            Hit();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "FallingZone")
        {
            //Hit(); //fall and death
        }
    }

    public void Hit()
    {
        if (isHit)
            return;
        HP--;
        if (HP <= 0 && !isDead)
        {
            animator.SetTrigger("IsDead");
            isDead = true;
        }
        GameManager.Instance.SetHealth(HP);
        flashCount = 0;
        isHit = true;
        FlashOn();
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
        if (flashCount < 4)
            Invoke("FlashOn", 0.2f);
        else
            isHit = false;
    }

    public void WalkEffect()
    {
        if (moving)
            Instantiate(WalkParticle, transform.position, transform.rotation);
    }
}
