using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionProjectile : MonoBehaviour, IHitable
{
    public GameObject MinionPrefabs;
    public float ThrowSpeedMin, ThrowSpeedMax;
    private float hSpeed, vSpeed;
    public ParticleSystem DeathParticleSystem;

    private void Awake()
    {
        hSpeed = Random.Range(ThrowSpeedMin, ThrowSpeedMax);
        vSpeed = Random.Range(-1f, 1f);
    }

    private void Update()
    {
        transform.Translate(hSpeed * Time.deltaTime * transform.localScale.x, vSpeed * Time.deltaTime, 0);
    }

    public void SpawnMinion()
    {
        Instantiate(MinionPrefabs, transform.position + new Vector3(0, 0.3f, 0), transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Weapon")
        {
            if (other.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Knife_Attack"))
            {
                Hit();
            }
        }
    }

    public void Hit()
    {
        DeathParticleSystem.gameObject.SetActive(true);
        DeathParticleSystem.transform.SetParent(null);
        Destroy(gameObject);
    }
}
