using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZone : MonoBehaviour
{
    public List<GameAction> GameActionList;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            spriteRenderer.enabled = true;
            foreach (var action in GameActionList)
            {
                if (action.ActionType == ActionType.Activate)
                    action.target.GetComponent<IAtcivatable>().Activate();
                else if (action.ActionType == ActionType.Spawn)
                    if (action.target) action.target.SetActive(true);
            }

        }
    }
}
