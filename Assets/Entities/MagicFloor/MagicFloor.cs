using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFloor : MonoBehaviour, IAtcivatable
{
    Collider2D collider2d;
    SpriteRenderer sprite;

    public void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
        Deactivate();
    }

    public void Activate()
    {
        sprite.enabled = true;
        collider2d.enabled = false;
    }

    public void Deactivate()
    {
        sprite.enabled = false;
        collider2d.enabled = true;
    }

}
