using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpener : MonoBehaviour
{
   public bool open;
    private Animator anmi;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        open = gameManager.gameComplete;
        anmi = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        open = gameManager.gameComplete;
        if (open)
        {
            anmi.SetBool("open", true);
            gameManager.gameComplete = false;
            col.enabled = true;

        }
        
        
    }
}
