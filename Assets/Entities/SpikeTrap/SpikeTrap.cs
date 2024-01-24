using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour, IAtcivatable
{
    public bool LoopMode = false;
    public float Interval = 2; 

    public bool IsOn = false;
    private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        if (LoopMode)
            InvokeRepeating("Toggle", Interval, Interval);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsOn", IsOn);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsOn)
            return;

        if (other.tag == "Player")
            other.GetComponent<Player>().Hit();
        else if (other.tag == "Enemy")
            other.GetComponent<IHitable>().Hit();
    }

    public void Activate()
    {
        Invoke("TurnOn", 0.35f);
    }

    public void Deactivate()
    {
        IsOn = false;
    }

    void TurnOn()
    {
        IsOn = true;
    }

    void Toggle()
    {
        IsOn = !IsOn;
    }
}
