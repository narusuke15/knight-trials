using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSight : MonoBehaviour
{
    bool spotted = false;
    public Transform owner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (spotted)
            return;

        if (other.gameObject.tag == "Player")
        {
            spotted = true;
            owner.GetComponent<IWakable>().Wake(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!spotted)
            return;
        if (other.gameObject.tag == "Player")
        {
            spotted = false;
            owner.GetComponent<IWakable>().Sleep();
        }
    }
}
