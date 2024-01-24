using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public int Speed = 5;
    public bool Parryable = true;
    public GameObject DeathParticle;
    Transform spriteTransform;

    private void Start()
    {
        spriteTransform = transform.Find("Sprite").transform;
        Destroy(gameObject, 5);
    }

    void Update()
    {
        transform.Translate(transform.localScale.x * 5 * Time.deltaTime, 0, 0);
        spriteTransform.Rotate(new Vector3(0, 0, -10), Space.Self);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Solid")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!Parryable)
            return;

        if (other.transform.tag == "Weapon")
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            gameObject.layer = 13;
            gameObject.tag = "PlayerProjectile";
        }
    }

    private void OnDestroy()
    {
        Instantiate(DeathParticle, transform.position, transform.rotation);
    }

}
