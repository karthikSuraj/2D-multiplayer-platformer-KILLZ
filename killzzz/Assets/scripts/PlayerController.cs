using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject leaveRoomButton;
    public Text playerNameText;
    public Text pingText;
    public Animator anmi;
    public float speed = 10; 
    public float jumpForce = 500;
    public SpriteRenderer sr;
    [SerializeField] GameObject Camera;
    bool ground;
    public GameObject BulletObject;
    public Transform BulletPosition;
    private float health=100 ;
    public Image Fillimage;
    public int score;
    private bool nextLevel = false;
    playerManager playerManager;
   GameObject[] coins;
    GameObject[] healthKits;
   GameObject[] enemies;
   Transform[] intialPositionOfEnemies;





    private Rigidbody2D rb;
    PhotonView PV;

    private void Awake()
        
    {
       
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int) PV.InstantiationData[0]).GetComponent<playerManager>();
     

        if (PV.IsMine)
        {
            anmi = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            playerNameText.text = PhotonNetwork.NickName;
            playerNameText.color = Color.green;
            Camera.SetActive(true);
            score = 0;
            health = 100;
        }
        else
        {
            playerNameText.text = PV.Owner.NickName;
            Camera.SetActive(false);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        coins = GameObject.FindGameObjectsWithTag("coin");
        healthKits= GameObject.FindGameObjectsWithTag("healthKit");
         enemies= GameObject.FindGameObjectsWithTag("enemy");
       
        for (int i = 0; i < enemies.Length; i++)
        {
           // intialPositionOfEnemies[i].position = enemies[i].transform.position;
           
        }

        health = 100;
        score = 0;


    }

    // Update is called once per frame
    void Update()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;

        ShownHealth();

        //PV.RPC("respwan", RpcTarget.AllBuffered);


        if (PV.IsMine)
        {

         
            
            gameManager.nameOfPlayer1 = PhotonNetwork.NickName;
            gameManager.scoreOfPlayer1 = score;

            pingText.text = "PING: " + PhotonNetwork.GetPing();
            if (Input.GetKey(KeyCode.Escape))
            {
                leaveRoomButton.SetActive(true);               
            }

           
            if (Input.GetKeyDown(KeyCode.Space) )
            {
                leaveRoomButton.SetActive(false);
            }
            if(health<=0)
            {
                
                PV.RPC("die", RpcTarget.AllBuffered);
                
            }
            else
            {
               
                
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Shoot();

                }
                PlayerMove();

            }
          

        }

        else
        {
            gameManager.scoreOfPlayer2 = score;
           
            gameManager.nameOfPlayer2 = PV.Owner.NickName;
            return;
        }      
       
    }
    void PlayerMove()
    {
        anmi.SetInteger("movement", 0);
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * speed * Time.deltaTime;
       

        if (Input.GetAxis("Horizontal") > 0)
        {
            anmi.SetInteger("movement", 1);
            PV.RPC("FlipFalse", RpcTarget.AllBuffered);


        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            anmi.SetInteger("movement", 1);
         
            PV.RPC("FlipTrue", RpcTarget.AllBuffered);

            // text.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        if ((  (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow)) ) && (ground) ) )

        {
            
            anmi.SetInteger("movement", 2);
            rb.AddForce(Vector2.up *jumpForce);

            //move = new Vector3(Input.GetAxisRaw("Horizontal"), 20);
            //transform.position += move * speed * Time.deltaTime;
        }
       
       
    }

    private void Shoot()
    {
        if(sr.flipX == false)
        {
            GameObject obj = PhotonNetwork.Instantiate(BulletObject.name, new Vector2(BulletPosition.position.x, BulletPosition.position.y), Quaternion.identity, 0);
            obj.SetActive(true);
        }
        if(sr.flipX == true)
        {
            GameObject obj = PhotonNetwork.Instantiate(BulletObject.name, new Vector2(BulletPosition.position.x, BulletPosition.position.y), Quaternion.identity, 0);
            obj.SetActive(true);
            obj.GetComponent<PhotonView>().RPC("ChangeDirection_left", RpcTarget.AllBuffered);
        }

    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            ground = true;
        }

        if (collision.CompareTag("enemy"))
        {
            health -= 0.1f;
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            ground = false;
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            health -= 1;
        }
        if (collision.CompareTag("water"))
        {
            health =0;
        }

        if (collision.CompareTag("door")  && SceneManager.GetActiveScene().buildIndex==1)
        {
            SceneManager.LoadScene(2);
            score = 0;
            gameManager.scoreOfPlayer1 = 0;
            gameManager.scoreOfPlayer2 = 0;
           
            //PhotonNetwork.LoadLevel(2);
        }

        if (collision.CompareTag("door") && SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(3);

            

            //PhotonNetwork.LoadLevel(2);
        }
        if (collision.CompareTag("healthKit"))
        {

           
            health += 50;
            if(health>=100)
            {
                health = 100;
            }
            collision.gameObject.SetActive(false);
        }


        if (collision.CompareTag("coin"))
        {

            score += 10;
            
            collision.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }
    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }

    void ShownHealth()
    {
        Fillimage.fillAmount = health / 100;
    }

    [PunRPC]
    void die()
    {
        if(PV.IsMine)
            {
            anmi.SetBool("die", true);
            

            PhotonNetwork.Destroy(gameObject);


            playerManager.die();
           

            score = 0;
            gameManager.instance.Reset();
           

        }
        else
        {
            return;
        }

     
       
        
      
    }

   

   








}
