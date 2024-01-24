using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Switch : MonoBehaviour
{
    public bool IsOn;
    public List<GameAction> GameActionList;
    Animator animator;

    private bool onDelay = false;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (onDelay)
            return;

        if (other.gameObject.tag == "Weapon")
        {
            IsOn = !IsOn;
            animator.SetBool("IsOn", IsOn);
            onDelay = true;
            CancelInvoke();
            Invoke("Delay", 0.3f);
            if (IsOn)
            {
                foreach (var action in GameActionList)
                {
                    if (action.ActionType == ActionType.Activate)
                        action.target.GetComponent<IAtcivatable>().Activate();
                    else if (action.ActionType == ActionType.Spawn)
                        if(action.target) action.target.SetActive(true);
                }
            }
            //off
            else
            {
                foreach (var action in GameActionList)
                {
                    if (action.ActionType == ActionType.Activate)
                        action.target.GetComponent<IAtcivatable>().Deactivate();
                }
            }
        }
    }

    void Delay()
    {
        onDelay = false;
    }
}
