using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public static groundCheck instance;
    public bool grounded;

    public void Awake()
    {
        instance = this;
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("ground"))
        {
            grounded = true;
        }
       
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            grounded = false;
        }
    }

    
}



