using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyController : MonoBehaviour
{
   
    int direction = 1; 
    [SerializeField] float speed=5;
    public GameObject target1,target2;
    private Animator anmi;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool dirct = false;
   public float EnemyHealth=100;
    public GameObject[] players;
    public float[] disx ;
    public float[] disy;
    public Image Fillimage;
    public bool startFromIdle;
    void Start()
    {
        anmi = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        disx = new float[5];
        disy = new float[5];

    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyHealth <= 0)
            Die();
        



        players = GameObject.FindGameObjectsWithTag("Player");
        ShowHealth();
        DistanceXCheck();
        DistanceYCheck();
        minimumDistance();
        if(startFromIdle)
        {
            anmi.SetInteger("movement", 0);           

        }
        else 
        {
            if (heightMatchCheack() && disx[minimumDistance()] <5 )
            {
                target1.SetActive(false);
                target2.SetActive(false);

               
                GoToPlayer();

            }
            else
            {
                if (target1.activeInHierarchy == false)
                {
                    setTriggers();
                }

              move();
            }
        }

        
        

        
    }

   

    public  Vector3 currentPos()
    {
        Vector3 currentPositon = transform.position;
        return currentPositon;
    }

    void setTriggers()
    {
        target1.SetActive(true);
        target1.transform.position = currentPos() + new Vector3(5, 0, 0);

        target2.SetActive(true);
        target2.transform.position = currentPos() + new Vector3(-5, 0, 0);
    }
    

    public void move()
    {

        speed = 5;
        anmi.SetInteger("movement", 1);

        transform.position += new Vector3(speed, 0,0)*direction*Time.deltaTime;

    }

    public void Attack()
    {
        speed = 0;
        anmi.SetInteger("movement",2);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ( ((collision.CompareTag("ground"))) || ((collision.CompareTag("enemy"))) )
        {
           
            direction *= -1;
            dirct = !dirct;
            sr.flipX = dirct;
        }
        if (collision.CompareTag("bullet"))
        {
           EnemyHealth -= 5;
            startFromIdle = false;
            
        }
        if (gameObject.transform.parent == collision.transform.parent  && (collision.CompareTag("triggers"))) 
        {
            direction *= -1;
            dirct = !dirct;
            sr.flipX = dirct;
        }

    }
    public void Die()
    {
        anmi.SetBool("die", true);
        StartCoroutine(DestroyByTime());
        speed = 0;
        
        
    }

   public void DistanceXCheck()
    {
        for (int i = 0; i < players.Length; i++)
        {
            
            float distance=0;
            distance = transform.position.x - players[i].transform.position.x;
            distance = Mathf.Abs(distance);
            disx[i] = distance;

           
        }
        
        
    }
    public void DistanceYCheck()
    {
        for (int i = 0; i < players.Length; i++)
        {
            float height = 0;
            height = transform.position.y - players[i].transform.position.y;
            height = Mathf.Abs(height);
            disy[i] = height;

           
        }
        

    }
    public bool heightMatchCheack()
    {
      
        if ( disy[minimumDistance()] <= 0.5)
        {
            return true;
        }
        else return false;
    }


    public int minimumDistance()
    {
        int pos = 0;
        float min = disx[0];
        for(int i = 0; i< players.Length;i++ )
        {
            if(min >disx[i])
            {
                min = disx[i];
                pos = i;
            }
        }

        return pos;
    }

    public void GoToPlayer()
    {
        if ((transform.position.x - players[minimumDistance()].transform.position.x) < 1.3f && (transform.position.x - players[minimumDistance()].transform.position.x) > -1.3f)
        {
            Attack();
        }

        else
        {
            speed = 5;
            if (sr.flipX== false) // right side facing 
            {
                direction = 1;
                dirct = false;
                if ((transform.position.x - players[minimumDistance()].transform.position.x) > 0) // player is front

                {
                    transform.position += new Vector3(speed, 0, 0) * Time.deltaTime * -1;
                    sr.flipX = true;
                }

                else if ((transform.position.x - players[minimumDistance()].transform.position.x) < 0) // player is back 
                {
                    transform.position += new Vector3(speed, 0, 0) * Time.deltaTime * 1;
                   
                }
            }
            if (sr.flipX == true) // right side facing 
            {
                direction = -1;
                dirct = true;
                if ((transform.position.x - players[minimumDistance()].transform.position.x) > 0) // player is front

                {
                    transform.position += new Vector3(speed, 0, 0) * Time.deltaTime * -1;
                    
                }

                else if ((transform.position.x - players[minimumDistance()].transform.position.x) < 0) // player is back 
                {
                    transform.position += new Vector3(speed, 0, 0) * Time.deltaTime * 1;
                    sr.flipX = false;
                }
            }


        }
        }
       
    void ShowHealth()
    {
        Fillimage.fillAmount = EnemyHealth / 100;
       
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(0.6f);
        this.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void DestroyObject()
    {
       this.gameObject.transform.parent.gameObject.SetActive(false);
    }


}


